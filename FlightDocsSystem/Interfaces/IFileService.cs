using FlightDocsSystem.Dtos;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FlightDocsSystem.Interfaces
{
    public interface IFileService
    {
        Task<InitFileResponse> UploadFile(IFormFile file, string fileFolder);

        //Task<string> UploadReportAsync(IFormFile file, DocumentCreateDto documentCreateDto);
        //Task<string> UploadSignatureAsync(IFormFile file, int flightId);
        Task<byte[]> DownloadFileAsync(string filePath);
        Task<InitFileResponse> UpdateReportFileDirectly(IFormFile newFile, int documentId);
        Task<bool> DeleteFileAsync(string filePath);
    }
}
