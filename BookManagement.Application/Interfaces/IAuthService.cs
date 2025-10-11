using BookManagement.Application.DTOs.Auth;

namespace BookManagement.Application.Interfaces;

public interface IAuthService
{
    Task<TokenResponseDto> RegisterAsync(RegisterDto registerDto);
    Task<TokenResponseDto> LoginAsync(LoginDto loginDto);
    Task<TokenResponseDto> RefreshTokenAsync(string refreshToken);
    Task<UserDto> GetCurrentUserAsync(int userId);
}