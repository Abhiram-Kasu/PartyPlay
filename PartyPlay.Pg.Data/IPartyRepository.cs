using SoapExtensions;

namespace PartyPlay.Pg.Data;

public interface IPartyRepository
{
    Task<List<Party>> GetPartiesUsersAsync();
    Task<Party?> GetPartyAsync(int id, bool includeUsers = false);
    Task<Result<Party, Error>> CreatePartyAsync(string name, int ownderId);
    public sealed record PartyDoesNotExistError() : Error("Party does not exist");

    Task<Result<bool, Error>> AddToPartyAsync(int partyId, int userId);
    Task<Result<bool, Error>> RemoveFromPartyAsync(int partyId, int userId);
    
}