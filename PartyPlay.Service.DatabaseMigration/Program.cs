global using PartyPlay.Pg.Data;
using PartyPlay.Service.DatabaseMigration;

var builder = Host.CreateApplicationBuilder(args);


builder.AddServiceDefaults();
builder.AddPgFeatures();
builder.Services.AddHostedService<Worker>();
builder.Services.AddOpenTelemetry().WithTracing(x => x.AddSource(Worker.ActivityName));
var host = builder.Build();
host.Run();