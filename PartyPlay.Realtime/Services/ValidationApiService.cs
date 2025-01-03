namespace PartyPlay.Realtime.Services;

public class ValidationApiService
{
    private static readonly string RestApiUrl = Environment.GetEnvironmentVariable("") ?? throw new Exception();
    private readonly HttpClient _httpClient = new(){BaseAddress = new Uri(RestApiUrl) };


    public async Task<bool> CheckUserExists(int userId) => await _httpClient.GetAsync($"/api/users/{userId}") is
        { IsSuccessStatusCode: true };
    //TODO Implement this api endpoint
    public async Task<bool> CheckPartyExists(int partyId) => await _httpClient.GetAsync($"/api/parties/{partyId}") is
        { IsSuccessStatusCode: true };

}