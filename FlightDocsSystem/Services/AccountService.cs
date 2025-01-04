using AutoMapper;
using FlightDocsSystem.Data;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightDocsSystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ICheckInput _checkInput;
        public AccountService(AppDbContext context, IMapper mapper, IPasswordHasher passwordHasher, ICheckInput checkInput)
        {
            _context = context;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _checkInput = checkInput;
        }

        public async Task<IEnumerable<AccountGetDto>> GetAllAsync()
        {
            var accounts = await _context.Accounts.ToListAsync();
            return _mapper.Map<IEnumerable<AccountGetDto>>(accounts);
        }

        public async Task<AccountGetDto> GetByIdAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            return account == null ? null : _mapper.Map<AccountGetDto>(account);
        }

        public async Task<AccountGetDto> CreateAsync(AccountCreateDto accountDto)
        {
          

            if (!_checkInput.CheckEmail(accountDto.Email))
                throw new Exception("Invalid email format.");

            if (!_checkInput.CheckPassword(accountDto.Password))
                throw new Exception("Invalid password format.");

            if (!_checkInput.CheckPhoneNumber(accountDto.Phone))
                throw new Exception("Invalid phone number format.");

            var account = _mapper.Map<Account>(accountDto);
            account.Password = _passwordHasher.HashPassword(account.Password);

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return _mapper.Map<AccountGetDto>(account);
        }


        public async Task<bool> UpdateAsync(int id, AccountUpdateDto accountDto)
        {
            try
            {
                var account = await _context.Accounts.FindAsync(id);
                if (account == null)
                    throw new Exception("Account not found.");

                if (!_checkInput.CheckEmail(accountDto.Email))
                    throw new Exception("Invalid email format.");
              
                if (!_checkInput.CheckPhoneNumber(accountDto.Phone))
                    throw new Exception("Invalid phone number format.");

                _mapper.Map(accountDto, account);

                _context.Accounts.Update(account);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update account: {ex.Message}");
            }
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null) return false;

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
