using System.Security.Claims;

namespace BookManagement.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(int userId, string phoneNumber, string role);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}