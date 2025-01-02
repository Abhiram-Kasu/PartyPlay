using Microsoft.EntityFrameworkCore;
using SoapExtensions;
using static PartyPlay.Pg.Data.IPartyUserRepository;

namespace PartyPlay.Pg.Data;

public class PgPartyUserRepository(ApplicationDbContext _context) : IPartyUserRepository
{

    public Task<List<PartyUser>> GetPartyUsersAsync() => _context.PartyUsers.ToListAsync();

    public async Task<Result<PartyUser, UserDoesNotExistError>> GetPartyUserAsync(int id)
    {
        if (await _context.PartyUsers.FindAsync(id) is { } user)
            return user;
        return new UserDoesNotExistError();
    }

    public async Task<Result<PartyUser, StandardError>> CreatePartyUserAsync(CreatePartyUserRequest request)
    {
        try
        {
            
            _context.PartyUsers.Add(request);
            await _context.SaveChangesAsync();
            return request.ToPartyUser();
        }
        catch (Exception e)
        {
            return new StandardError(e.Message, e);
        }
    }

    public async Task<Result<bool, Error>> UpdatePartyUserAsync(int id, PartyUser user)
    {
        if (await _context.PartyUsers.FindAsync(id) is not { } existingUser) return new UserDoesNotExistError();
        existingUser.Name = user.Name;
        existingUser.PartyId = user.PartyId;
        existingUser.IsOwner = user.IsOwner;
        try
        {

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new StandardError(ex.Message, ex);
        }
        return true;
    }

    public async Task<Result<bool, Error>> DeletePartyUserAsync(int id)
    {
        if (await _context.PartyUsers.FindAsync(id) is not { } user) return new UserDoesNotExistError();
        _context.PartyUsers.Remove(user);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return new StandardError(ex.Message, ex);
        }
        return true;
    }
}