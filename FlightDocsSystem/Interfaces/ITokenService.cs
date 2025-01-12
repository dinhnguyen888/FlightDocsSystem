using System.Security.Claims;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Models;
namespace FlightDocsSystem.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(AccountGetDto accountDto);
        ClaimsPrincipal ValidateToken(string token);
        string GetClaimValue(string token, string claimType);
        string GetTokenFromHTTPContext(IHttpContextAccessor context);
        string GetRoleNameFromToken(IHttpContextAccessor contextAccessor);
        Task<string> GetEmailFromToken(IHttpContextAccessor contextAccessor);
    }

}
