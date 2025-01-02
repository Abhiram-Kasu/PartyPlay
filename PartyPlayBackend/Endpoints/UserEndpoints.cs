using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PartyPlayBackend.Endpoints;

public static class UserEndpointsExtensions
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapPost("/users/create", UserEndpoints.CreateUser);
        
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

    
    static async Task<IResult> CreateUser([FromServices]ILogger<UserEndpoints> _logger, ApplicationDbContext context,
        [FromBody] CreateUserRequest request)
    {
        _logger.LogInformation("Creating user with name: {name}", request.Name);
        var user = new PartyUser
        {
            Name = request.Name,
            IsOwner = false
        };
        context.PartyUsers.Add(user);

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Failed to create user: {ex}", ex);
            return Results.InternalServerError();
        }
        


        return Results.Ok(new CreateUserResponse(user.Id));
    }

    sealed record AddUserToPartyRequest(int PartyId, int UserId);
    static async Task<IResult> AddUserToParty(ILogger<UserEndpoints> _logger, ApplicationDbContext context, [FromBody]AddUserToPartyRequest request )
    {
        _logger.LogInformation("Trying to add user {userId} to party {partyId}", request.UserId, request.PartyId);
        var party = await context.Parties.Include(p => p.Users).FirstOrDefaultAsync(p => p.Id == request.PartyId);
        if (party is null)
        {
            _logger.LogWarning("Party {partyId} not found", request.PartyId);
            return Results.NotFound();
        }
        var user = await context.PartyUsers.FirstOrDefaultAsync(u => u.Id == request.UserId);
        if (user is null)
        {
            _logger.LogWarning("User {userId} not found", request.UserId);
            return Results.NotFound();
        }
        party.Users.Add(user);
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Failed to add user to party: {ex}", ex);
            return Results.InternalServerError();
        }
        return Results.Ok();
    }
}