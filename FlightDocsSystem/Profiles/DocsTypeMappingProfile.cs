using AutoMapper;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Models;

namespace FlightDocsSystem.Profiles
{
    public class DocsTypeMappingProfile : Profile
    {
        public DocsTypeMappingProfile() 
        {
            CreateMap<DocsTypeCreateDto, DocsType>();
            CreateMap<DocsType, DocsTypeGetDto>()
               .ForMember(dest => dest.PermissionCount,
                          opt => opt.MapFrom(src => src.Permissions.Count));
            CreateMap<DocsTypeUpdateDto, DocsType>();
           
        }
    }
}
