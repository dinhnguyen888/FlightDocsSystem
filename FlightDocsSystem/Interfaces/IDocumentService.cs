using FlightDocsSystem.Dtos;

namespace FlightDocsSystem.Interfaces
{
    public interface IDocumentService
    {
        Task<List<DocumentGetDto>> GetAsync();
        Task<List<DocumentGetDto>> GetOriginalDocsAsync();
        Task<List<DocumentGetDto>> GetUpdatedDocsAsync();
        Task<DocumentGetDto> GetByIdAsync(int documentId);
        Task<DocumentGetDto> CreateAsync(DocumentCreateDto documentCreateDto, IFormFile file);
        Task<DocumentGetDto> UpdateAsync(int documentId, DocumentUpdateDto documentUpdateDto, IFormFile? file, IHttpContextAccessor contextAccessor);
        Task<bool> DeleteAsync(int documentId);

    }
}
