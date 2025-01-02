using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;

namespace PartyPlay.Service.DatabaseMigration;

public class Worker(IServiceProvider _serviceProvider, ILogger<Worker> _logger, IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
  
    internal const string ActivityName = "MigrationService";
    private static readonly ActivitySource _activitySource = new (ActivityName);
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var activity = _activitySource.StartActivity("Migrating Database", ActivityKind.Server);
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync(stoppingToken);
        }
        catch (Exception e)
        {
            activity.RecordException(e);
            throw;

        }
        hostApplicationLifetime.StopApplication();
    }
}