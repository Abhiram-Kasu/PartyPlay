using Microsoft.EntityFrameworkCore;
using SoapExtensions;

namespace PartyPlay.Pg.Data;

internal class PgPartyRepository(ApplicationDbContext _context) : IPartyRepository
{
    public async Task<List<Party>> GetPartiesUsersAsync() => await _context.Parties.ToListAsync();

    public async Task<Party?> GetPartyAsync(int id, bool includeUsers = false)
    {
        var party = await _context.Parties.FindAsync(id);
        if (party is null) return null;
        if (includeUsers)
        {
            await _context.Entry(party).Collection(p => p.Users).LoadAsync();
        }

        return party;
    }

    public Task<Party?> CreatePartyAsync(Party party)
    {
        throw new NotImplementedException();
    }


    public async Task<Result<bool, Error>> AddToPartyAsync(int partyId, int userId)
    {
        //Assumes that the user and partyId exists
        var party = await _context.Parties.FindAsync(partyId);
        if (party is null)
        {
            return new IPartyUserRepository.UserDoesNotExistError();
        }
        var partyUser = await _context.PartyUsers.FindAsync(userId);
        if (partyUser is null)
        {
            return new IPartyRepository.PartyDoesNotExistError();
        }
        
        party.Users.Add(partyUser);
        try
        {
            await _context.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            return new StandardError("Something went wrong with saving changes", ex);
        }

        return true;
    }

    public Task<bool> RemoveFromPartyAsync(int partyId, int userId)
    {
        throw new NotImplementedException();
    }
}