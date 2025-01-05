using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SoapExtensions;

namespace PartyPlayBackend.Endpoints;

public static class UserEndpointsExtensions
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapPost("/users/create", UserEndpoints.CreateUser);
        app.MapGet("/users/{id:int}", UserEndpoints.GetUser);
        app.MapPost("/users/addToParty", UserEndpoints.AddUserToParty);
    }
}

/*
 * Design decision to separate the functions from the static class to give the `T` in ILogger<T> a more specific type
 * This was also intentionally made an interface to exploit default interface methods
 * and to enforce a lack of state. There should only be functions or definitions in this interface
 */
// ReSharper disable once InconsistentNaming
internal interface UserEndpoints
{
    sealed record CreateUserRequest(string Name);

    sealed record CreateUserResponse(int Id);


    static async Task<IResult> CreateUser([FromServices] ILogger<UserEndpoints> _logger,
        [FromServices] IPartyUserRepository repository,
        [FromBody] CreateUserRequest request)
    {
        _logger.LogInformation("Creating user with name: {name}", request.Name);


        switch (await repository.CreatePartyUserAsync(new(request.Name, null)))
        {
            case { IsSuccess: false, Error: StandardError error }:
                _logger.LogCritical("Failed to create user: {ex}", error.Exception);
                return Results.InternalServerError();
            case { IsSuccess: true, Value: { } user }:
                return Results.Ok(new CreateUserResponse(user.Id));
            default:
                _logger.LogCritical("Unexpected result from repository");
                return Results.InternalServerError();
        }
        
    }

    sealed record AddUserToPartyRequest(int PartyId, int UserId);

    static async Task<IResult> AddUserToParty(ILogger<UserEndpoints> _logger,
        [FromServices] IPartyRepository partyRepository,
        [FromServices] IPartyUserRepository userRepository,
        [FromBody] AddUserToPartyRequest request)
    {
        _logger.LogInformation("Trying to add user {userId} to party {partyId}", request.UserId, request.PartyId);

        var res = await partyRepository.AddToPartyAsync(request.PartyId, request.UserId);

        
        if(res.IsSuccess) return Results.Ok();
        switch (res.Error)
        {
            case IPartyUserRepository.UserDoesNotExistError: 
                _logger.LogWarning("User {userId} not found", request.UserId);
                return Results.NotFound();
            case IPartyRepository.PartyDoesNotExistError:
                _logger.LogWarning("Party {partyId} not found", request.PartyId);
                return Results.NotFound();
            case StandardError error:
                _logger.LogCritical("Failed to add user to party, {ex}", error.Exception);
                return Results.InternalServerError();
            default:
                _logger.LogCritical("Unexpected result from repository");
                return Results.InternalServerError();
        }
        

    }

    static async Task<IResult> GetUser([FromServices] ILogger<UserEndpoints> _logger,
        [FromServices] IPartyUserRepository repository,
        [FromQuery] int id)
    {
        _logger.LogInformation("Getting user with id: {id}", id);
        var res = await repository.GetPartyUserAsync(id);
        if (res.Value is {} user) return Results.Ok(user);
        _logger.LogWarning("User {id} not found", id);
        return Results.NotFound();
    }
}