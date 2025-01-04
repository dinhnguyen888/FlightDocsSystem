﻿using AutoMapper;
using FlightDocsSystem.Data;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlightDocsSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        public AuthService(ITokenService tokenService, AppDbContext context, IMapper mapper, IPasswordHasher passwordHasher)
        {
            _tokenService = tokenService;
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var account = await AuthenticateUser(email, password);
            if (account == null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var token = _tokenService.GenerateToken(_mapper.Map<AccountGetDto>(account));

            return token;
        }

        public async Task LogoutAsync(string sessionToken)
        {
            await Task.CompletedTask;
        }

        public async Task<string> RefreshTokenAsync(string oldSessionToken)
        {
            var principal = _tokenService.ValidateToken(oldSessionToken);
            if (principal == null)
                throw new UnauthorizedAccessException("Invalid token.");

            var accountDto = new AccountGetDto
            {
                Id = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Email = principal.FindFirst(ClaimTypes.Email)?.Value,
                Name = principal.FindFirst(ClaimTypes.Name)?.Value,
                IsActive = bool.Parse(principal.FindFirst("IsActive")?.Value),
                PermissionName = principal.FindFirst(ClaimTypes.Role)?.Value,
                
             
            };

            var newToken = _tokenService.GenerateToken(accountDto);

            return newToken;
        }

        public async Task ChangeActiveSessionAsync(int userId)
        {
            // Tìm tài khoản trong cơ sở dữ liệu
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Id == userId);
            if (account == null)
                throw new UnauthorizedAccessException("Account not found.");

            // Cập nhật trạng thái IsActive
            account.IsActive = !account.IsActive;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.Accounts.Update(account);
            await _context.SaveChangesAsync();
        }



        private async Task<AccountGetDto?> AuthenticateUser(string email, string password)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(e => e.Email ==email);
            if (account == null)
            {
                return null;
            }
            var checkPassword = _passwordHasher.VerifyPassword(password, account.Password);

            return _mapper.Map<AccountGetDto>(account);

        }

    }
}
