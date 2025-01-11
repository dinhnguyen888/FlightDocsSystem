using AutoMapper;
using FlightDocsSystem.Data;
using FlightDocsSystem.DTOs;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FlightDocsSystem.Services
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RoleService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleGetDto>> GetAllAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            return _mapper.Map<IEnumerable<RoleGetDto>>(roles); 
        }

        public async Task<RoleGetDto> GetByIdAsync(int id)
        {
            var role = await _context.Roles.SingleOrDefaultAsync(x => x.Id == id);
            if (role == null)
            {
                throw new ArgumentException("Role not found");
            }
            return _mapper.Map<RoleGetDto>(role); 
        }


        public async Task<RoleGetDto> CreateAsync(RoleCreateDto role)
        {
            var roleMapping = _mapper.Map<Role>(role);
            roleMapping.CreateDate = DateTime.Now;
            var existing = await _context.Roles.AnyAsync(r => r.RoleName == roleMapping.RoleName);
            if (existing) throw new DuplicateNameException("Duplicate Name!!! ");

            _context.Roles.Add(roleMapping);
            await _context.SaveChangesAsync();

            return _mapper.Map<RoleGetDto>(role);
        }

        public async Task<RoleGetDto> UpdateAsync(int id, RoleUpdateDto roleUpdate)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)  throw new ArgumentException("invalid RoleId ");
            _mapper.Map(roleUpdate, role);

            _context.Roles.Update(role);
            await _context.SaveChangesAsync();

            return _mapper.Map<RoleGetDto>(role);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) throw new ArgumentException("invalid RoleId");

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
