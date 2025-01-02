using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var dbServer = builder.AddPostgres("dbServer").WithPgAdmin();
var db = dbServer.AddDatabase("partyPlay");

var kafka  = builder.AddKafka("partyMessaging").WithKafkaUI().WithDataVolume(isReadOnly:false);
builder.AddProject<PartyPlay_Service_DatabaseMigration>("partyPlayMigration").WithReference(db);
builder.AddProject<PartyPlayBackend>("partyPlayBackend").WithReference(kafka).WithReference(db);

builder.Build().Run();