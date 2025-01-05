using Microsoft.AspNetCore.Mvc;
using SoapExtensions;

namespace PartyPlayBackend.Endpoints;

public static class PartyEndpointsExtensions
{
    public static void MapPartyEndpoints(this WebApplication app)
    {
        app.MapPost("/parties/create", PartyEndpoints.CreateParty);
        app.MapGet("/parties/{id:int}", PartyEndpoints.GetParty);

    }
}

internal interface PartyEndpoints
{
    sealed record CreatePartyRequest(string Name, int OwnerId);
    public static async Task<IResult> CreateParty([FromBody] CreatePartyRequest request, [FromServices] ILogger<PartyEndpoints> _logger,
    [FromServices] IPartyRepository repository)
    {
        _logger.LogInformation("Creating party with name: {name}", request.Name);
        var (name, ownerId) = request;
        switch (await repository.CreatePartyAsync(name, ownerId))
        {
            case { IsSuccess: false, Error: StandardError error }:
                _logger.LogCritical("Failed to create party: {ex}", error.Exception);
                return Results.InternalServerError();
            case { IsSuccess: true, Value: { } party }:
                return Results.Ok(party);
            default:
                _logger.LogCritical("Unexpected result from repository");
                return Results.InternalServerError();
        }
        
    }

    public static async Task<IResult> GetParty([FromQuery] int id, [FromServices] ILogger<PartyEndpoints> _logger,
        [FromServices] IPartyRepository partyRepository)
    {
        _logger.LogInformation("Getting party with id: {id}", id);
        var res = await partyRepository.GetPartyAsync(id, true);
        if (res is not null) return Results.Ok(res);
        _logger.LogWarning("Party with id {id} does not exist", id);
        return Results.NotFound();

    }
    
    
 
}