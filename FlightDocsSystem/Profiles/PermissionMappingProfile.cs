using AutoMapper;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Models;

namespace FlightDocsSystem.Profiles
{
    public class PermissionMappingProfile : Profile
    {
        public PermissionMappingProfile()
        {
            CreateMap<Permission, PermissionGetDto>()
                .ForMember(dest => dest.DocsTypeName, opt => opt.MapFrom(src => src.DocsType.DocsTypeName))
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));

            CreateMap<PermissionCreateDto, Permission>();
            CreateMap<PermissionUpdateDto, Permission>();
            CreateMap<Permission, PermissionGetForDocsType>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName));
               
        }
    }
}
