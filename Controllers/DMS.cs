using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ASCO.Services;
using ASCO.DTOs;
using Microsoft.Extensions.Options;



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
}