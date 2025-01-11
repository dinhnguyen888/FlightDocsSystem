using AutoMapper;
using FlightDocsSystem.DTOs;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using FlightDocsSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var groupPermissions = await _roleService.GetAllAsync();
                return Ok(groupPermissions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving roles.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var groupPermission = await _roleService.GetByIdAsync(id);
                if (groupPermission == null)
                    return NotFound(new { message = $"Role with ID {id} not found." });

                return Ok(groupPermission);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the role.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoleCreateDto roleCreateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _roleService.CreateAsync(roleCreateDto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (DuplicateNameException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the role.", details = ex.Message });
            }
           
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RoleUpdateDto roleUpdateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var updated = await _roleService.UpdateAsync(id, roleUpdateDto);
                if (updated == null)
                    return NotFound(new { message = $"Role with ID {id} not found." });

                return Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the role.", details = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _roleService.DeleteAsync(id);
                if (!deleted)
                    return NotFound(new { message = $"Role with ID {id} not found." });

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the role.", details = ex.Message });
            }
        }
    }

}
