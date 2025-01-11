using AutoMapper;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Models;

namespace FlightDocsSystem.Profiles
{
    public class FlightMappingProfile : Profile
    {
        public FlightMappingProfile() 
        {
            CreateMap<Flight, FlightGetDto>()
                .ForMember(dest => dest.FlightStatuses, opt => opt.MapFrom(src => src.FlightStatus))
                .ForMember(dest => dest.Documents, opt => opt.MapFrom(dest => dest.Documents))
                .ForMember(dest => dest.DocumentCount, opt => opt.MapFrom(dest => dest.Documents.Count()));
            CreateMap<FlightUpdateDto,Flight>();
            CreateMap<FlightCreateDto, Flight>();
           
        }

    }
}
