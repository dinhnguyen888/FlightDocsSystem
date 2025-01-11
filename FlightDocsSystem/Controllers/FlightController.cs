using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }
        [Authorize(Policy = "AllowAll")]
        [HttpGet]
        public async Task<IActionResult> GetAllFlights()
        {
            var flights = await _flightService.GetAsync();
            return Ok(flights);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<IActionResult> CreateFlight([FromBody] FlightCreateDto flightCreateDto)
        {
            if (flightCreateDto == null)
                return BadRequest("Flight details are required.");

            try
            {
                var createdFlight = await _flightService.CreateAsync(flightCreateDto);
                return CreatedAtAction(nameof(GetAllFlights), new { id = createdFlight.Id }, createdFlight);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("{flightId}")]
        public async Task<IActionResult> UpdateFlight(int flightId, [FromBody] FlightUpdateDto flightUpdateDto)
        {
            if (flightUpdateDto == null)
                return BadRequest("Flight details are required.");

            try
            {
                var updatedFlight = await _flightService.UpdateAsync(flightId, flightUpdateDto);
                return Ok(updatedFlight);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Policy = "AllowAll")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchFlight([FromQuery] string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return BadRequest("Keyword is required.");

            try
            {
                var flights = await _flightService.SearchInFlight(keyword);
                return Ok(flights);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Policy = "PilotOnly")]
        [HttpPost("signature")]
        public async Task<IActionResult> UploadSignature([FromForm] FlightUploadSignatureDto upload)
        {
            try
            {
                var uploadSignature = await _flightService.UploadSignatureAsync(upload.flightId, upload.file);
                return Ok(uploadSignature);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            } 
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{flightId}")]
        public async Task<IActionResult> DeleteFlight(int flightId)
        {
            try
            {
                var isDeleted = await _flightService.DeleteAsync(flightId);
                if (isDeleted)
                    return NoContent();

                return NotFound("Flight not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
