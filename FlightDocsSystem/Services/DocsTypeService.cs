using AutoMapper;
using FlightDocsSystem.Data;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightDocsSystem.Services
{
    public class DocsTypeService : IDocsTypeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DocsTypeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DocsTypeGetDto>> GetAsync()
        {
            var docTypes = await _context.DocsTypes
                .Include(d => d.Permissions)
                .ThenInclude(p =>p.Role)
                .ToListAsync();
            return _mapper.Map<List<DocsTypeGetDto>>(docTypes);
        }

        public async Task<DocsTypeGetDto> CreateAsync(DocsTypeCreateDto docsTypeDto)
        {
            if (docsTypeDto == null) throw new ArgumentNullException("doctype can not be null");
            var docsType = _mapper.Map<DocsType>(docsTypeDto);
            _context.DocsTypes.Add(docsType);
            await _context.SaveChangesAsync();
            return _mapper.Map<DocsTypeGetDto>(docsType);
        }

        public async Task<bool> UpdateDocsTypeAsync(int docsTypeId, DocsTypeUpdateDto dto)
        {
            var docsType = await _context.DocsTypes.FindAsync(docsTypeId);
            if (docsType == null) throw new ArgumentException("DocsType not found");

            _mapper.Map(dto, docsType);
            _context.DocsTypes.Update(docsType);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteDocsTypeAsync(int docsTypeId)
        {
            var docsType = await _context.DocsTypes.FindAsync(docsTypeId);
            if (docsType == null) throw new ArgumentException("DocsType not found");

            _context.DocsTypes.Remove(docsType);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePermissionDocsType(int docsTypeId, PermissionCreateDto dto)
        {
            var docsType = await _context.DocsTypes
                .Include(d => d.Permissions)
                .FirstOrDefaultAsync(d => d.Id == docsTypeId);

            if (docsType == null) throw new ArgumentException("DocsType not found");

            var permission = _mapper.Map<Permission>(dto);
            docsType.Permissions.Add(permission);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
