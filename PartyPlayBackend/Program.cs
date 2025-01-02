
global using PartyPlay.Pg.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.AddPgFeatures();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", async (IPartyRepository repository) =>
{
    var parties = await repository.GetPartiesAsync();
    return Results.Ok(parties);
});



app.Run();

