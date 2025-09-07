using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASCO.Services;
using ASCO.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.StaticFiles;
//using MimeTypes;

//[Authorize]
[ApiController]
[Route("document")]
public class DMSController : ControllerBase
{
    private readonly DocumentService _docService;
    private readonly string _basePath;

    public DMSController(DocumentService dmsService, IOptions<DocumentStorageOptions> storageOptions)
    {
        _docService = dmsService;
        _basePath = storageOptions.Value.BasePath; //lets us configure the base path how ever we want it.

    }

    [HttpPost("upload")] //is different from create but create is still a part of it.
    public async Task<IActionResult> UploadDocument([FromForm] UploadFileDto dto)
    {
        var file = await _docService.UploadFileAsync(dto);
        return Ok(file);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateDocument([FromBody] CreateFolderDto dto)
    {
        var folder = await _docService.CreateFolderAsync(dto);
        return Ok(folder);
    }

    [HttpGet("all/hierarchy")] //this is to make the view for the file structure on the frontend.
    public async Task<IActionResult> GetAllDocsByHierarchy()
    {
        var docs = await _docService.GetAllByHierarchyAsync();
        return Ok(docs);
    }

    [HttpGet("fetch/{id}")]
    public async Task<IActionResult> GetFile(Guid id)
    {
        var (doc, filePath) = await _docService.GetDocumentWithFileAsync(id);

        if (doc == null || string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            return NotFound(new { message = "Document not found" });

        // var contentType = "application/octet-stream"; // fallback
        // var ext = Path.GetExtension(filePath);
        // if (!string.IsNullOrEmpty(ext))
        // {
        //     contentType = MimeTypeMap.GetMimeType(Path.GetExtension(filePath));
        // }
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(filePath, out string? contentType))
        {
            contentType = "application/octet-stream"; // fallback
        }
        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        //Response.Headers.Add("Content-Disposition", $"inline; filename={doc.Name}");
        Response.Headers.Append("Content-Disposition", $"inline; filename={doc.Name}");

        return File(fileBytes, contentType);
    }


    //forms and creations
    [HttpPost("form/create")]
    public async Task<IActionResult> CreateForm([FromBody] FormTemplateDTO dto)
    {
        var form = await _docService.CreateFormAsync(dto);
        return Ok(form);
    }

    [HttpGet("forms/templates")]
    public async Task<IActionResult> GetAllFormTemplates()
    {
        var forms = await _docService.GetAllFormTemplatesAsync();
        return Ok(forms);
    }

    [HttpGet("form/template/{id}")]
    public async Task<IActionResult> GetFormTemplateById(int id)
    {
        var form = await _docService.GetFormTemplateByIdAsync(id);
        if (form == null) return NotFound(new { message = "Form not found" });
        return Ok(form);
    }

    //a post method in order to store form responses in json files.
}