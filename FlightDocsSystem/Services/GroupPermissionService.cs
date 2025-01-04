using FlightDocsSystem.Data;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightDocsSystem.Services
{
    public class GroupPermissionService : IGroupPermissionService
    {
        private readonly AppDbContext _context;

        public GroupPermissionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GroupPermission>> GetAllAsync()
        {
            return await _context.GroupPermissions.ToListAsync();
        }

        public async Task<GroupPermission> GetByIdAsync(int id)
        {
            return await _context.GroupPermissions.FindAsync(id);
        }

        public async Task<GroupPermission> CreateAsync(GroupPermission groupPermission)
        {
            groupPermission.CreateDate = DateTime.UtcNow;
            _context.GroupPermissions.Add(groupPermission);
            await _context.SaveChangesAsync();
            return groupPermission;
        }

        public async Task<GroupPermission> UpdateAsync(int id, GroupPermission groupPermission)
        {
            var existing = await _context.GroupPermissions.FindAsync(id);
            if (existing == null) return null;

            existing.GroupPermissionName = groupPermission.GroupPermissionName;
            existing.Note = groupPermission.Note;
            existing.Creator = groupPermission.Creator;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var groupPermission = await _context.GroupPermissions.FindAsync(id);
            if (groupPermission == null) return false;

            _context.GroupPermissions.Remove(groupPermission);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
