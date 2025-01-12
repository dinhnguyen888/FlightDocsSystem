using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // 1. Tạo Token
    public string GenerateToken(AccountGetDto accountDto)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, accountDto.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, accountDto.Id.ToString()),
            new Claim(ClaimTypes.Name, accountDto.Name),
            new Claim("IsActive", accountDto.IsActive.ToString()),
            new Claim(ClaimTypes.Role, accountDto.RoleName)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiresInMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // 2. Xác Thực Token
    public ClaimsPrincipal ValidateToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = key,
            NameClaimType = ClaimTypes.NameIdentifier
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null; // Invalid token
        }
    }

    // 3. Lấy Giá Trị Từ Claim
    public string GetClaimValue(string token, string claimType)
    {
        var principal = ValidateToken(token);
        if (principal == null) return null;

        var claim = principal.FindFirst(claimType);
        return claim?.Value;
    }

    public string GetTokenFromHTTPContext(IHttpContextAccessor contextAccessor)
    {
        var context = contextAccessor.HttpContext;
        if (context == null)
            throw new UnauthorizedAccessException("HTTP context is null.");

        var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        if (string.IsNullOrWhiteSpace(token))
            throw new UnauthorizedAccessException("Token is missing or invalid.");

        return token;
    }
    public async Task<string> GetEmailFromToken(IHttpContextAccessor contextAccessor)
    {
        if (contextAccessor.HttpContext == null || contextAccessor.HttpContext.User == null)
            throw new UnauthorizedAccessException("HTTP context or User is null.");
        var emailClaim = contextAccessor.HttpContext.User.Claims
               .FirstOrDefault(c => c.Type == ClaimTypes.Email);
        if (emailClaim == null) throw new UnauthorizedAccessException("do not have a token or token expired");
        return emailClaim.Value;
    }
    public string GetRoleNameFromToken(IHttpContextAccessor contextAccessor)
    {
        if (contextAccessor.HttpContext == null || contextAccessor.HttpContext.User == null)
            throw new UnauthorizedAccessException("HTTP context or User is null.");

        var roleNameClaim = contextAccessor.HttpContext.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Role); 

        if (roleNameClaim == null)
            throw new UnauthorizedAccessException("RoleName not found in token.");

        return roleNameClaim.Value;
    }

}
