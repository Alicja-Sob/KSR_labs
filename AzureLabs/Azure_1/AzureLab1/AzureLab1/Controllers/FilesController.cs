using AzureTableAuthApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IAuthorizationService _authService;
    private readonly IFileService _fileService;

    public FilesController(IAuthorizationService authService, IFileService fileService)
    {
        _authService = authService;
        _fileService = fileService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveFile(
            [FromBody] FileOperationsRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Login) ||
            string.IsNullOrWhiteSpace(request.SessionId) ||
            string.IsNullOrWhiteSpace(request.FileName))
        {
            return BadRequest("Missing required data.");
        }

        var validSession = await _authService.ValidateSessionAsync(
            request.Login,
            request.SessionId
        );

        if (!validSession)
        {
            return Unauthorized("Invalid session.");
        }

        await _fileService.SaveFileAsync(
            request.FileName,
            request.Content
        );

        return Ok(new
        {
            message = "File saved successfully",
            fileName = request.FileName
        });
    }
    [HttpGet("{name}")]
    public async Task<IActionResult> GetFile(
            string name,
            [FromQuery] string login,
            [FromQuery] string sessionId)
    {
        if (string.IsNullOrWhiteSpace(name) ||
            string.IsNullOrWhiteSpace(login) ||
            string.IsNullOrWhiteSpace(sessionId))
        {
            return BadRequest("Missing required parameters.");
        }

        var valid = await _authService.ValidateSessionAsync(login, sessionId);

        if (!valid)
            return Unauthorized("Invalid session.");

        var content = await _fileService.ReadFileAsync(name);

        if (content == null)
            return NotFound("File not found.");

        return Ok(new
        {
            fileName = name,
            content
        });
    }
}