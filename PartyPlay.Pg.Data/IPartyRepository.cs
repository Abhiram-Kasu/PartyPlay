using SoapExtensions;

namespace PartyPlay.Pg.Data;

public interface IPartyRepository
{
    Task<List<Party>> GetPartiesUsersAsync();
    Task<Party?> GetPartyAsync(int id, bool includeUsers = false);
    Task<Party?> CreatePartyAsync(Party party);
    public sealed record PartyDoesNotExistError() : Error("Party does not exist");

    Task<Result<bool, Error>> AddToPartyAsync(int partyId, int userId);
    Task<bool> RemoveFromPartyAsync(int partyId, int userId);
    
}