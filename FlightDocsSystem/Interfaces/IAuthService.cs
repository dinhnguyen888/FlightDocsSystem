namespace FlightDocsSystem.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);
        Task LogoutAsync(string sessionToken);
        Task<string> RefreshTokenAsync(string oldSessionToken);
        Task ChangeActiveSessionAsync(string userId);
        Task<bool> ChangeOwner(string currentOwnerEmail, string newOwnerEmail);
    }
}
