using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocsTypeController : ControllerBase
    {
        private readonly IDocsTypeService _docsTypeService;

        public DocsTypeController(IDocsTypeService docsTypeService)
        {
            _docsTypeService = docsTypeService;
        }
        [Authorize(Policy = "AllowAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _docsTypeService.GetAsync();
            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DocsTypeCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _docsTypeService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DocsTypeUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _docsTypeService.UpdateDocsTypeAsync(id, dto);
            if (!success) return NotFound();

            return NoContent();
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _docsTypeService.DeleteDocsTypeAsync(id);
            if (!success) return NotFound();

            return NoContent();
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("{id}/permissions")]
        public async Task<IActionResult> UpdatePermission(int id, [FromBody] PermissionCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _docsTypeService.UpdatePermissionDocsType(id, dto);
            if (!success) return NotFound();

            return NoContent();
        }
    }
}