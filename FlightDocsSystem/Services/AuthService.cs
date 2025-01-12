using AutoMapper;
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
        private readonly ICheckInput _checkInput;
        public AuthService(ITokenService tokenService, AppDbContext context, IMapper mapper, IPasswordHasher passwordHasher, ICheckInput checkInput)
        {
            _tokenService = tokenService;
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _checkInput = checkInput;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var account = await AuthenticateUser(email, password);
            if (account == null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var token =  _tokenService.GenerateToken(_mapper.Map<AccountGetDto>(account));

            return token;
        }

        public async Task LogoutAsync(string sessionToken)
        {
            await Task.CompletedTask;
        }

        public async Task<string> RefreshTokenAsync(string oldSessionToken)
        {
            var principal =  _tokenService.ValidateToken(oldSessionToken);
            if (principal == null)
                throw new UnauthorizedAccessException("Invalid token.");

            var accountDto = new AccountGetDto
            {
                Id = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                Email = principal.FindFirst(ClaimTypes.Email)?.Value,
                Name = principal.FindFirst(ClaimTypes.Name)?.Value,
                IsActive = bool.Parse(principal.FindFirst("IsActive")?.Value),
                RoleName = principal.FindFirst(ClaimTypes.Role)?.Value,
                
             
            };

            var newToken = _tokenService.GenerateToken(accountDto);

            return newToken;
        }

        public async Task ChangeActiveSessionAsync(string userId)
        {
            // Tìm tài khoản trong cơ sở dữ liệu
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Id.ToString() == userId);
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

        public async Task<string> ResetPasswordAsync(string email, string newPassword)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Email == email);
            if (account == null)
                throw new ArgumentException("Email can not found");

            var hashedPassword = _passwordHasher.HashPassword(newPassword);
            account.Password = hashedPassword;

            _context.Update(account);
            await _context.SaveChangesAsync();
            return newPassword;
        }


        public async Task<bool> ChangeOwner(string currentOwnerEmail, string newOwnerEmail)
        {
            var ownerRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Owner");
            if (ownerRole == null)
                throw new Exception("Owner role not found in the database");

            var accounts = await _context.Accounts
                .Include(a => a.Role)
                .Where(a => a.Email == currentOwnerEmail || a.Email == newOwnerEmail)
                .ToListAsync();

            var currentOwner = accounts.FirstOrDefault(a => a.Email == currentOwnerEmail && a.Role.RoleName == "Owner");
            var newOwner = accounts.FirstOrDefault(a => a.Email == newOwnerEmail);

            if (currentOwner == null)
                throw new UnauthorizedAccessException($"Current owner '{currentOwnerEmail}' not found or this is not an Owner");
            if (newOwner == null)
                throw new UnauthorizedAccessException($"New owner '{newOwnerEmail}' not found");

            // Vô hiệu hóa current owner
            currentOwner.IsActive = false;

            // Ngắt quan hệ hiện tại của newOwner với Role
            newOwner.Role = null;

            // Lưu thay đổi để ngắt liên kết trước
            await _context.SaveChangesAsync();

            // Thiết lập vai trò mới cho newOwner
            newOwner.Role = ownerRole;

            // Lưu thay đổi cuối cùng
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
