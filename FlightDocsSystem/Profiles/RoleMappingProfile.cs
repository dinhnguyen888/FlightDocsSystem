using FlightDocsSystem.DTOs;
using FlightDocsSystem.Models;
using AutoMapper;


namespace FlightDocsSystem.Profiles
{
    public class RoleMappingProfile :Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<Role, RoleGetDto>();
            CreateMap<RoleCreateDto, Role>();
            CreateMap<RoleUpdateDto, Role>();
            CreateMap<RoleCreateDto, RoleGetDto>();
        }
    }
}
