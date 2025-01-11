using AutoMapper;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        [HttpGet("{docsTypeId}/{roleId}")]
        public async Task<IActionResult> GetPermissionById(int docsTypeId, int roleId)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(docsTypeId, roleId);
            if (permission == null) return NotFound();
            return Ok(permission);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionCreateDto permissionCreateDto)
        {
            await _permissionService.CreatePermissionAsync(permissionCreateDto);
            return CreatedAtAction(nameof(GetPermissionById), new { permissionCreateDto.DocsTypeId, permissionCreateDto.RoleId }, permissionCreateDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePermission([FromBody] PermissionUpdateDto permissionUpdateDto)
        {
            await _permissionService.UpdatePermissionAsync(permissionUpdateDto);
            return NoContent();
        }

        [HttpDelete("{docsTypeId}/{roleId}")]
        public async Task<IActionResult> DeletePermission(int docsTypeId, int roleId)
        {
            await _permissionService.DeletePermissionAsync(docsTypeId, roleId);
            return NoContent();
        }
    }
}

