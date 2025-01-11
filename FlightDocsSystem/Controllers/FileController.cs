using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FlightDocsSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        //[HttpPost("upload/report")]
        //public async Task<IActionResult> UploadReportAsync([FromForm] FileUploadRequest fileUploadRequest)
        //{
        //    if (fileUploadRequest.file == null || fileUploadRequest.document == null)
        //        return BadRequest("File and document data are required.");

        //    try
        //    {
        //        var filePath = await _fileService.UploadReportAsync(fileUploadRequest.file, fileUploadRequest.document);
        //        return Ok(new { Message = "Report uploaded successfully.", FilePath = filePath });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}

        //[HttpPost("upload/signature")]
        //public async Task<IActionResult> UploadSignatureAsync([FromForm] IFormFile file, [FromForm] int flightId)
        //{
        //    if (file == null || flightId <= 0)
        //        return BadRequest("File and valid flight ID are required.");

        //    try
        //    {
        //        var filePath = await _fileService.UploadSignatureAsync(file, flightId);
        //        return Ok(new { Message = "Signature uploaded successfully.", FilePath = filePath });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}

        //[HttpGet("download")]
        //public async Task<IActionResult> DownloadFileAsync([FromQuery] string fileName, [FromQuery] string fileDestination)
        //{
        //    if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(fileDestination))
        //        return BadRequest("File name and destination are required.");

        //    try
        //    {
        //        var fileBytes = await _fileService.DownloadFileAsync(fileName, fileDestination);
        //        return File(fileBytes, "application/octet-stream", fileName);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}

        //[HttpPut("update")]
        //public async Task<IActionResult> UpdateFileDirectly([FromForm] IFormFile newFile, [FromForm] string fileFolder, [FromForm] int documentId)
        //{
        //    if (newFile == null || string.IsNullOrWhiteSpace(fileFolder) || documentId <= 0)
        //        return BadRequest("New file, file folder, and valid document ID are required.");

        //    try
        //    {
        //        var filePath = await _fileService.UpdateFileDirectly(newFile, fileFolder, documentId);
        //        return Ok(new { Message = "File updated successfully.", FilePath = filePath });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}

        //[HttpDelete("delete")]
        //public async Task<IActionResult> DeleteFileAsync([FromQuery] string fileName, [FromQuery] string fileDestination)
        //{
        //    if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(fileDestination))
        //        return BadRequest("File name and destination are required.");

        //    try
        //    {
        //        var result = await _fileService.DeleteFileAsync(fileName, fileDestination);
        //        if (!result)
        //            return NotFound("File not found or could not be deleted.");

        //        return Ok(new { Message = "File deleted successfully." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //}
    }
}
