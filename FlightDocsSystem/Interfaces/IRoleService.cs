using FlightDocsSystem.DTOs;
using FlightDocsSystem.Models;

namespace FlightDocsSystem.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleGetDto>> GetAllAsync();
        Task<RoleGetDto> GetByIdAsync(int id);
        Task<RoleGetDto> CreateAsync(RoleCreateDto groupPermission);
        Task<RoleGetDto> UpdateAsync(int id, RoleUpdateDto groupPermission);
        Task<bool> DeleteAsync(int id);
    }
}
