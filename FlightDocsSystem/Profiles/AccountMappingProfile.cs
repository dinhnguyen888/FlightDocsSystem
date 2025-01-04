using AutoMapper;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Models;

namespace FlightDocsSystem.Profiles
{
    public class AccountMappingProfile : Profile
    {
        public AccountMappingProfile()
        {
            CreateMap<Account, AccountGetDto>();

            CreateMap<AccountCreateDto, Account>();

            CreateMap<AccountUpdateDto, Account>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

        }
    }
}
