using FlightDocsSystem.DTOs;
using FlightDocsSystem.Models;
using AutoMapper;


namespace FlightDocsSystem.Profiles
{
    public class GroupPermissionMappingProfile :Profile
    {
        public GroupPermissionMappingProfile()
        {
            CreateMap<GroupPermission, GroupPermissionGetDto>();
            CreateMap<GroupPermissionCreateDto, GroupPermission>();
            CreateMap<GroupPermissionUpdateDto, GroupPermission>();
        }
    }
}
