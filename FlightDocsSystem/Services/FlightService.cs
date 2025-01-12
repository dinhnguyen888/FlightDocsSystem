using AutoMapper;
using FlightDocsSystem.Data;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore;

namespace FlightDocsSystem.Services
{
    public class FlightService : IFlightService
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;
        
        public FlightService(IMapper mapper, AppDbContext context, IFileService fileService)
        {
            _mapper = mapper;
            _context = context;
            _fileService = fileService;
        }

        public async Task<IEnumerable<FlightGetDto>> GetAsync()
        {
            var flights = await _context.Flights
            .Include(f => f.Documents)
            .ThenInclude(f => f.DocsType)
            .ToListAsync();
            var flightMapper = _mapper.Map<IEnumerable<FlightGetDto>>(flights);
            return flightMapper;
        }

        public async Task<FlightGetDto> CreateAsync(FlightCreateDto flightCreateDto)
        {
            if(flightCreateDto == null) throw new ArgumentNullException("some fields is missing");
           
            var flight = _mapper.Map<Flight>(flightCreateDto);
            if (flight.ArrivalDate < flight.DepartureDate) throw new ArgumentException("Arrival Date must be behind of Departure date");
            _context.Add(flight);
            await _context.SaveChangesAsync();
            return _mapper.Map<FlightGetDto>(flight);
        }

        public async Task<FlightGetDto> UpdateAsync(int flightId, FlightUpdateDto flightUpdateDto)
        {
            var existing = await _context.Flights.FirstOrDefaultAsync(f => f.Id == flightId);
            if (existing == null) throw new ArgumentException("invalid flightId");
            _mapper.Map(flightUpdateDto, existing);
            _context.Update(existing);
             await _context.SaveChangesAsync();
            return _mapper.Map<FlightGetDto>(existing);

        }


        public async Task<List<Flight>> SearchInFlight(string keyword)
        {
            var flights = await _context.Flights
                .Include(f => f.Documents)
                .ThenInclude(f => f.DocsType)
                .Where(f => f.FlightNo == keyword
                    || f.Documents.Any(d => d.DocumentName.Contains(keyword)))
                .ToListAsync();

            return flights;
        }

        public async Task<bool> UploadSignatureAsync(int flightId, IFormFile signature)
        {
            var existing = await _context.Flights.SingleOrDefaultAsync(f => f.Id == flightId);
            if (existing == null) throw new ArgumentException("can not find flightId");
            if (existing.FlightStatus == FlightStatuses.End) throw new ArgumentException("Flight has signature already");
            var uploadSignature = await _fileService.UploadFile(signature, "uploads/Signature");
            existing.SignaturePath = uploadSignature.filePath;
            existing.FlightStatus = FlightStatuses.End;
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<string> GetSignatureById(int flightId)
        //{
        //    var existing = await _context.Flights.SingleOrDefaultAsync(f => f.Id == flightId);
        //    if (existing == null)
        //        throw new Exception("Invalid flight ID.");

        //    if (string.IsNullOrEmpty(existing.SignaturePath))
        //        throw new Exception("No signature found for this flight.");

        //    return existing.SignaturePath;
        //}


        public async Task<bool> DeleteAsync(int flightId)
        {
            var existing = await _context.Flights.FirstOrDefaultAsync(f => f.Id==flightId);
            if (existing == null) throw new ArgumentException("Invalid flightID");
            _context.Remove(existing);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}

