using FlightDocsSystem.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightDocsSystem.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountGetDto>> GetAllAsync();
        Task<AccountGetDto> GetByIdAsync(int id);
        Task<AccountGetDto> CreateAsync(AccountCreateDto accountDto);
        Task<bool> UpdateAsync(int id, AccountUpdateDto accountDto);
        Task<bool> DeleteAsync(int id);
    }
}
