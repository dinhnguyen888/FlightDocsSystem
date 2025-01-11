using AutoMapper;
using FlightDocsSystem.Data;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightDocsSystem.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PermissionService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionGetDto>> GetAllPermissionsAsync()
        {
            // Lấy danh sách toàn bộ Permission và ánh xạ sang DTO
            var permissions = await _context.Permissions
                .Include(p => p.DocsType)
                .Include(p => p.Role)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PermissionGetDto>>(permissions);
        }

        public async Task<PermissionGetDto> GetPermissionByIdAsync(int docsTypeId, int roleId)
        {
            // Tìm Permission theo khóa chính (DocsTypeId và RoleId)
            var permission = await _context.Permissions
                .Include(p => p.DocsType)
                .Include(p => p.Role  )
                .FirstOrDefaultAsync(p => p.DocsTypeId == docsTypeId && p.RoleId == roleId);

            return permission == null ? null : _mapper.Map<PermissionGetDto>(permission);
        }

        public async Task CreatePermissionAsync(PermissionCreateDto permissionCreateDto)
        {
            // Tạo Permission mới từ DTO
            var permission = _mapper.Map<Permission>(permissionCreateDto);

            // Thêm và lưu vào cơ sở dữ liệu
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePermissionAsync(PermissionUpdateDto permissionUpdateDto)
        {
            // Tìm Permission cần cập nhật
            var permission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.DocsTypeId == permissionUpdateDto.DocsTypeId && p.RoleId == permissionUpdateDto.RoleId);

            if (permission == null)
                throw new ArgumentException("Permission not found.");

            // Cập nhật giá trị từ DTO và lưu thay đổi
            _mapper.Map(permissionUpdateDto, permission);
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePermissionAsync(int docsTypeId, int roleId)
        {
            // Tìm Permission cần xóa
            var permission = await _context.Permissions
                .FirstOrDefaultAsync(p => p.DocsTypeId == docsTypeId && p.RoleId == roleId);

            if (permission == null)
                throw new ArgumentException("Permission not found.");

            // Xóa và lưu thay đổi
            _context.Permissions.Remove(permission);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsAllowAccessDocument(string roleName, int docsTypeId, List<string> permissions, List<string> rolePassAnyway)
        {
            if (rolePassAnyway.Contains(roleName)) return true;

            var roleId = await _context.Roles
                .Where(r => r.RoleName == roleName)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (roleId == 0)
                throw new ArgumentException("Invalid RoleName provided.");

            var matchingPermissions = await _context.Permissions
                .Where(p => p.DocsTypeId == docsTypeId && p.RoleId == roleId)
                .ToListAsync();

            var isAllowed = matchingPermissions.Any(p => permissions.Contains(p.PermissionType.ToString()));

            if (!isAllowed)
                throw new ArgumentException("You don't have permission to access this method!");

            return isAllowed;
        }
    }
}
