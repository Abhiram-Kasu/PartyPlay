using Microsoft.AspNetCore.Mvc;


namespace PartyPlay.Realtime.RealtimeEndpoints;


public static class RealtimePartyEndpointsExtensions
{
    public static void AddRealtimePartyEndpoints(this WebApplication app)
    {
        app.MapGet("/party", RealtimePartyEndpoints.JoinPartyAsync);
    }
}




public interface RealtimePartyEndpoints
{
    static async Task<IResult> JoinPartyAsync([FromHeader]int partyId,[FromHeader] int userId, HttpContext context )
    {
        if(!context.WebSockets.IsWebSocketRequest) return Results.BadRequest("Not a websocket request");
        //Check that partyId and userId are valid
        
        var socket = await context.WebSockets.AcceptWebSocketAsync();
        
        
        
        return Results.Ok();
    }
}