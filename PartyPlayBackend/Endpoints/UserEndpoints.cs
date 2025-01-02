using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PartyPlayBackend.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapPost("/users/create", CreateUser);
        
    }

    sealed record CreateUserRequest(string Name);

    sealed record CreateUserResponse(int Id);

    static async Task<IResult> CreateUser(ILogger _logger, ApplicationDbContext context,
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
    static async Task<IResult> AddUserToParty(ILogger _logger, ApplicationDbContext context, [FromBody]AddUserToPartyRequest request )
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