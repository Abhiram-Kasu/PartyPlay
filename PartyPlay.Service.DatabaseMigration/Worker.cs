using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;

namespace PartyPlay.Service.DatabaseMigration;

public class Worker(
    IServiceProvider _serviceProvider,
    ILogger<Worker> _logger,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
    internal const string ActivityName = "MigrationService";
    private static readonly ActivitySource _activitySource = new(ActivityName);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = _activitySource.StartActivity("Migrating Database", ActivityKind.Server);
        const uint numberOfRetries = 10;
        uint currentRetry = 0;
        while (currentRetry < numberOfRetries)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.MigrateAsync(stoppingToken);
                break;
            }
            catch (Exception e)
            {
                activity?.RecordException(e);
                _logger.LogError("Failed to migrate database: {e}", e);
            }

            _logger.LogCritical("Failed to migrate database, retrying in {numSeconds} seconds", currentRetry+1);
            await Task.Delay(TimeSpan.FromSeconds(currentRetry), stoppingToken);
            if(currentRetry++ == numberOfRetries)
            {
                _logger.LogCritical("Failed to migrate database after {numberOfRetries} retries, stopping application", numberOfRetries);
                hostApplicationLifetime.StopApplication();
                return;
            }
            _logger.LogCritical("Starting retry attempt {currentRetry}", currentRetry+1);
            currentRetry++;
        }

        hostApplicationLifetime.StopApplication();
    }
}