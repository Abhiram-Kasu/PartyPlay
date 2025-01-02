namespace PartyPlay.Pg.Data;

public interface IPartyRepository
{
    Task<List<Party>> GetPartiesUsersAsync();
    Task<Party?> GetPartyAsync(int id);
    Task<Party?> CreatePartyAsync(Party party);
    Task<bool> AddToPartyAsync(int partyId, PartyUser user);
    Task<bool> RemoveFromPartyAsync(int partyId, int userId);
    
}