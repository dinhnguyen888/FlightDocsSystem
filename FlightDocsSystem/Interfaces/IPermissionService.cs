using FlightDocsSystem.Dtos;

namespace FlightDocsSystem.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionGetDto>> GetAllPermissionsAsync();
        Task<PermissionGetDto> GetPermissionByIdAsync(int docsTypeId, int roleId);
        Task CreatePermissionAsync(PermissionCreateDto permissionCreateDto);
        Task UpdatePermissionAsync(PermissionUpdateDto permissionUpdateDto);
        Task DeletePermissionAsync(int docsTypeId, int roleId);
        Task<bool> IsAllowAccessDocument(string roleName, int docsTypeId, List<string> permission , List<string> rolePassAnyway);
    }
}
