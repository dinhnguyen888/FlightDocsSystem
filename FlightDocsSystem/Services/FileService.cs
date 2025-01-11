using AutoMapper;
using FlightDocsSystem.Data;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

namespace FlightDocsSystem.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FileService(IWebHostEnvironment environment, AppDbContext context, IMapper mapper)
        {
            _environment = environment;
            _context = context;
            _mapper = mapper;
        }

        public async Task<InitFileResponse> UploadFile(IFormFile file, string fileFolder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid File.");

            string fileName = Path.GetFileName(file.FileName);
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Invalid File Name.");

            string uploadPath = Path.Combine(_environment.WebRootPath, fileFolder);
            Directory.CreateDirectory(uploadPath);

            // XU ly trung ten 
            string filePath = Path.Combine(uploadPath, fileName);
            string fileExtension = Path.GetExtension(fileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

            int counter = 1;
            while (File.Exists(filePath))
            {
                fileName = $"{fileNameWithoutExtension}_{counter}{fileExtension}";
                filePath = Path.Combine(uploadPath, fileName);
                counter++;
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var response = new InitFileResponse
            {
                fileName = fileName,
                filePath = filePath,
            };
            return response;
        }

        public async Task<byte[]> DownloadFileAsync(string filePath)
        {
            if (filePath.Contains(".."))
                throw new ArgumentException("Invalid file path.");

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.");

            return await File.ReadAllBytesAsync(filePath);
        }


        public async Task<InitFileResponse> UpdateReportFileDirectly(IFormFile newFile, int documentId)
        {
            string fileFolder = "uploads/Report";

            if (newFile == null || newFile.Length == 0)
                throw new ArgumentException("Invalid file.");

            var existingFile = await _context.Documents.SingleOrDefaultAsync(d => d.DocumentId == documentId);
            if (existingFile == null)
                throw new ArgumentException("Document ID not found.");

            var deleteOldFileInSystem = await this.DeleteFileAsync(existingFile.DocumentPath);
            if (!deleteOldFileInSystem)
                Console.WriteLine($"Warning: Old file {existingFile.DocumentName} could not be deleted.");

            var fileInited = await UploadFile(newFile, fileFolder);
        

            return fileInited;
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            try
            {
                await Task.Run(() => File.Delete(filePath));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file {filePath}: {ex.Message}");
                return false;
            }

            return true;
        }



    }
}
