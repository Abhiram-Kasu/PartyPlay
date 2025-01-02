using SoapExtensions;

namespace PartyPlay.Pg.Data;

public interface IPartyUserRepository
{
    Task<List<PartyUser>> GetPartyUsersAsync();
    sealed record UserDoesNotExistError() : Error("User does not exist");
    Task<Result<PartyUser, UserDoesNotExistError>> GetPartyUserAsync(int id);

    sealed record CreatePartyUserRequest(string Name, int? PartyId, bool IsOwner = false)
    {
        public static implicit operator PartyUser(CreatePartyUserRequest req) => new PartyUser
        {
            Name = req.Name,
            PartyId = req.PartyId,
            IsOwner = req.IsOwner
        };
        public PartyUser ToPartyUser() => this;
    };
    Task<Result<PartyUser, StandardError>> CreatePartyUserAsync(CreatePartyUserRequest request);

    Task<Result<bool, Error>> UpdatePartyUserAsync(int id, PartyUser user);
    
    Task<Result<bool, Error>> DeletePartyUserAsync(int id);
    
    
    
}