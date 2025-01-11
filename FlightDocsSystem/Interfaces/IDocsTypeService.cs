using FlightDocsSystem.Dtos;

namespace FlightDocsSystem.Interfaces
{
    public interface IDocsTypeService
    {
        Task<List<DocsTypeGetDto>> GetAsync();
        Task<DocsTypeGetDto> CreateAsync(DocsTypeCreateDto dto);
        Task<bool> UpdateDocsTypeAsync(int docsTypeId,DocsTypeUpdateDto dto); 
        Task<bool> DeleteDocsTypeAsync(int  docsTypeId);
        Task<bool> UpdatePermissionDocsType(int docsTypeId,PermissionCreateDto dto );
    }
}
