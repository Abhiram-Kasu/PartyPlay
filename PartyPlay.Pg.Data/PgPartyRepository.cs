using Microsoft.EntityFrameworkCore;

namespace PartyPlay.Pg.Data;

internal class PgPartyRepository(ApplicationDbContext _context) : IPartyRepository
{
    public async Task<List<Party>> GetPartiesUsersAsync() => await _context.Parties.ToListAsync();

    public Task<Party?> GetPartyAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Party?> CreatePartyAsync(Party party)
    {
        throw new NotImplementedException();
    }

    public Task<bool> AddToPartyAsync(int partyId, PartyUser user)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveFromPartyAsync(int partyId, int userId)
    {
        throw new NotImplementedException();
    }
}