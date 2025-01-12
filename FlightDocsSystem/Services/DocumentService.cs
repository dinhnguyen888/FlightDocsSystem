using AutoMapper;
using FlightDocsSystem.Data;
using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FlightDocsSystem.Services
{
    public class DocumentService : IDocumentService
    {
  
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ITokenService _tokenService;
        private readonly IPermissionService _permissionService;
        public DocumentService( AppDbContext appDbContext, IMapper mapper, IFileService fileService, ITokenService tokenService, IPermissionService permissionService)
        {
            _appDbContext = appDbContext; 
            _mapper = mapper;
            _fileService = fileService;
            _tokenService = tokenService;
            _permissionService = permissionService;
            
        }

        public async Task<List<DocumentGetDto>> GetAsync()
        {
            var documents = await _appDbContext.Documents
                .Include(d => d.DocsType).ToListAsync();
            return _mapper.Map<List<DocumentGetDto>>(documents);
        }
        public async Task<List<DocumentGetDto>> GetOriginalDocsAsync()
        {
            var documents = await _appDbContext.Documents
                .Include(f => f.DocsType)
                .Where(d => d.LastestVersion == 1)
                .ToListAsync();

            return _mapper.Map<List<DocumentGetDto>>(documents);
        }
        public async Task<List<DocumentGetDto>> GetUpdatedDocsAsync()
        {
            var documents = await _appDbContext.Documents
                .Include(f => f.DocsType)
                .Where(d => d.LastestVersion != 1)
                .ToListAsync();

            return _mapper.Map<List<DocumentGetDto>>(documents);
        }


        public async Task<DocumentGetDto> GetByIdAsync(int documentId)
        {
            var document = await _appDbContext.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
            if (document == null) throw new ArgumentException("can not found documentID");
            return _mapper.Map<DocumentGetDto>(document);
        }

        public async Task<DocumentGetDto> CreateAsync(DocumentCreateDto documentCreateDto, IFormFile file)
        {
            if (documentCreateDto == null)
                throw new ArgumentNullException("Fill full the document field");

            var documentWithDocsTypeId = await _appDbContext.DocsTypes
                .SingleOrDefaultAsync(d => d.Id == documentCreateDto.DocsTypeId);

            if (documentWithDocsTypeId == null)
                throw new Exception("DocsTypeId not found");

            var documentWithFlightId = await _appDbContext.Flights
                .SingleOrDefaultAsync(f => f.Id == documentCreateDto.FlightId);
           
            if (documentWithFlightId == null)
                throw new Exception("FlightId not found");
            if (documentWithFlightId.FlightStatus == FlightStatuses.End) throw new Exception("flight has been end, cannot edit or add.");

            var uploadFile = await _fileService.UploadFile(file, "uploads/Report");

           if (documentCreateDto.DocumentName.IsNullOrEmpty())
                documentCreateDto.DocumentName = Path.GetFileNameWithoutExtension(uploadFile.fileName);
            
         
            if (uploadFile == null)
                throw new ArgumentException("File doesn't match.");

            var document = _mapper.Map<Document>(documentCreateDto);
            document.CreateDate = DateTime.UtcNow;
            document.DocumentPath = uploadFile.filePath;

            _appDbContext.Documents.Add(document);
            await _appDbContext.SaveChangesAsync();

            return _mapper.Map<DocumentGetDto>(document);
        }

        public async Task<DocumentGetDto> UpdateAsync(int documentId, DocumentUpdateDto documentUpdateDto,IFormFile? file, IHttpContextAccessor contextAccessor)
        {

            // lay roleName tu token
            var roleName = _tokenService.GetRoleNameFromToken(contextAccessor);
            // quyen duoc truy cap vao phuong thuc
            var requiredPermissions = new List<string> { "ReadAndModify" };
            var rolePassAnyway = new List<string> { "Admin" };
            //check documentId co ton tai khong 
            var isDocumentExisting = await _appDbContext.Documents.Include(d => d.Flight).SingleOrDefaultAsync(d => d.DocumentId == documentId);
           
            if (isDocumentExisting == null) throw new ArgumentException("documentId can not found");
            // check xem role cua user nay co quyen vao document nay khong
            await _permissionService.IsAllowAccessDocument(roleName, isDocumentExisting.DocsTypeId, requiredPermissions, rolePassAnyway);

            // neu chuyen bay ket thuc, khong the them tai lieu vao dc
            if (isDocumentExisting.Flight.FlightStatus == FlightStatuses.End) throw new ArgumentException("flight has been end. Cannot edit or add ");

            if (file != null || (file != null && documentUpdateDto.DocumentName != null))
            {
                var updateDocument = await _fileService.UpdateReportFileDirectly(file, documentId);
                documentUpdateDto.DocumentName = updateDocument.fileName;
                documentUpdateDto.DocumentPath = updateDocument.filePath;
            }
            
            _mapper.Map(documentUpdateDto, isDocumentExisting);

            isDocumentExisting.LastestVersion = isDocumentExisting.LastestVersion + 0.1;
            _appDbContext.Update(isDocumentExisting);
            await _appDbContext.SaveChangesAsync();
            return _mapper.Map<DocumentGetDto>(isDocumentExisting);
        }

        public async Task<bool> DeleteAsync(int documentId)
        {
            var documentExisting = await _appDbContext.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
            if (documentExisting == null) throw new KeyNotFoundException("documentId not found");
            var removeDocument = await _fileService.DeleteFileAsync(documentExisting.DocumentPath);
            _appDbContext.Remove(documentExisting);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
