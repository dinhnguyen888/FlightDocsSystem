using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightDocsSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;
      
        private readonly IHttpContextAccessor _httpContext;

        public DocumentsController(IDocumentService documentService , IHttpContextAccessor httpContext)
        {
            _documentService = documentService;
             
            _httpContext = httpContext;
        }

        // Get all documents
        [Authorize(Policy = "PilotAndAdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
           
            var documents = await _documentService.GetAsync();
            return Ok(documents);
        }

        [Authorize(Policy = "PilotAndAdminOnly")]
        [HttpGet("originDocs")]
        public async Task<IActionResult> GetOriginDocuments()
        {

            var documents = await _documentService.GetOriginalDocsAsync();
            return Ok(documents);
        }

        [Authorize(Policy = "PilotAndAdminOnly")]
        [HttpGet("updatedDocs")]
        public async Task<IActionResult> GetUpdatedDocuments()
        {

            var documents = await _documentService.GetUpdatedDocsAsync();
            return Ok(documents);
        }

        [Authorize(Policy = "PilotAndAdminOnly")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            try
            {
                var document = await _documentService.GetByIdAsync(id);
                return Ok(document);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [Authorize(Policy = "PilotAndAdminOnly")]
        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromForm] DocumentUploadDto documentUploadDto)
        {
            try
            {

                if (documentUploadDto.file == null)
                    return BadRequest("File must be provided.");

                var newDocument = await _documentService.CreateAsync(documentUploadDto.documentCreateDto, documentUploadDto.file);
                return CreatedAtAction(nameof(GetDocumentById), new { id = newDocument.DocumentId }, newDocument);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Policy = "AllowAll")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(int id, [FromForm] DocumentUpdateFileDto documentUpdateFileDto)
        {
            try
            {

                var updatedDocument = await _documentService.UpdateAsync(id, documentUpdateFileDto.documentUpdateDto, documentUpdateFileDto.file, _httpContext);
                return Ok(updatedDocument);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            try
            {
                var isDeleted = await _documentService.DeleteAsync(id);
                if (isDeleted)
                    return NoContent();
                return BadRequest("Failed to delete document.");
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    }
}
