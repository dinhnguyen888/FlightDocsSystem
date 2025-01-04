using FlightDocsSystem.Models;

namespace FlightDocsSystem.Interfaces
{
    public interface IGroupPermissionService
    {
        Task<IEnumerable<GroupPermission>> GetAllAsync();
        Task<GroupPermission> GetByIdAsync(int id);
        Task<GroupPermission> CreateAsync(GroupPermission groupPermission);
        Task<GroupPermission> UpdateAsync(int id, GroupPermission groupPermission);
        Task<bool> DeleteAsync(int id);
    }
}
