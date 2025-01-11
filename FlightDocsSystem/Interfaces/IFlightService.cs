using FlightDocsSystem.Dtos;
using FlightDocsSystem.Models;

namespace FlightDocsSystem.Interfaces
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightGetDto>> GetAsync();
        Task<FlightGetDto> CreateAsync(FlightCreateDto flightCreateDto);
        Task<List<Flight>> SearchInFlight(string searchWord);
        Task<FlightGetDto> UpdateAsync(int flightId,FlightUpdateDto dto);

        Task<bool> UploadSignatureAsync(int flightId, IFormFile signature);
        Task<bool> DeleteAsync(int flightId);
    }
}
