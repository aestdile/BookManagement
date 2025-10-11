using AutoMapper;
using BookManagement.Application.DTOs.Auth;
using BookManagement.Application.Exceptions;
using BookManagement.Application.Interfaces;
using BookManagement.Domain.Entities;
using BookManagement.Domain.Enums;
using BookManagement.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BookManagement.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IMapper mapper,
        IConfiguration configuration)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<TokenResponseDto> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userRepository.PhoneNumberExistsAsync(registerDto.PhoneNumber))
        {
            throw new UserAlreadyExistsException();
        }

        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            PhoneNumber = registerDto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            Role = UserRole.User,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryDays = int.Parse(_configuration["JwtSettings:RefreshTokenExpirationInDays"] ?? "7");

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.PhoneNumber, user.Role.ToString());

        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60")),
            User = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<TokenResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByPhoneNumberAsync(loginDto.PhoneNumber);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
        {
            throw new InvalidCredentialsException();
        }

        if (!user.IsActive)
        {
            throw new InvalidCredentialsException("Foydalanuvchi faol emas");
        }

        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.PhoneNumber, user.Role.ToString());
        var refreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryDays = int.Parse(_configuration["JwtSettings:RefreshTokenExpirationInDays"] ?? "7");

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();

        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60")),
            User = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var user = await _userRepository.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new InvalidCredentialsException("Token yaroqsiz yoki muddati o'tgan");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(user.Id, user.PhoneNumber, user.Role.ToString());
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        var refreshTokenExpiryDays = int.Parse(_configuration["JwtSettings:RefreshTokenExpirationInDays"] ?? "7");

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        _userRepository.Update(user);
        await _userRepository.SaveChangesAsync();

        return new TokenResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationInMinutes"] ?? "60")),
            User = _mapper.Map<UserDto>(user)
        };
    }

    public async Task<UserDto> GetCurrentUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            throw new NotFoundException("Foydalanuvchi", userId);
        }

        return _mapper.Map<UserDto>(user);
    }
}