using AutoMapper;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Models;

namespace FlightDocsSystem.Profiles
{
    public class DocumentMappingProfile :Profile
    {
        public DocumentMappingProfile() 
        {
            CreateMap<DocumentCreateDto, Document>();
            CreateMap<Document, DocumentGetDto>()
                .ForMember(dest => dest.DocsTypeName, opt => opt.MapFrom(src => src.DocsType.DocsTypeName));
            CreateMap<DocumentUpdateDto, Document>()
                .ForMember(dest => dest.DocsType, opt => opt.Ignore());
        }
    }
}
