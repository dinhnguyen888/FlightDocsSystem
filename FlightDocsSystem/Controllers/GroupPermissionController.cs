using AutoMapper;
using FlightDocsSystem.DTOs;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using FlightDocsSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupPermissionController : ControllerBase
    {
        private readonly IGroupPermissionService _groupPermissionService;
        private readonly IMapper _mapper;

        public GroupPermissionController(IGroupPermissionService groupPermissionService, IMapper mapper)
        {
            _groupPermissionService = groupPermissionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groupPermissions = await _groupPermissionService.GetAllAsync();
            var groupPermissionDtos = _mapper.Map<IEnumerable<GroupPermissionGetDto>>(groupPermissions);
            return Ok(groupPermissionDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var groupPermission = await _groupPermissionService.GetByIdAsync(id);
            if (groupPermission == null) return NotFound();

            var groupPermissionDto = _mapper.Map<GroupPermissionGetDto>(groupPermission);
            return Ok(groupPermissionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] GroupPermissionCreateDto groupPermissionCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var groupPermission = _mapper.Map<GroupPermission>(groupPermissionCreateDto);
            var created = await _groupPermissionService.CreateAsync(groupPermission);
            var groupPermissionReadDto = _mapper.Map<GroupPermissionGetDto>(created);

            return CreatedAtAction(nameof(GetById), new { id = groupPermissionReadDto.Id }, groupPermissionReadDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] GroupPermissionUpdateDto groupPermissionUpdateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var groupPermission = _mapper.Map<GroupPermission>(groupPermissionUpdateDto);
            var updated = await _groupPermissionService.UpdateAsync(id, groupPermission);

            if (updated == null) return NotFound();

            var groupPermissionReadDto = _mapper.Map<GroupPermissionGetDto>(updated);
            return Ok(groupPermissionReadDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _groupPermissionService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
