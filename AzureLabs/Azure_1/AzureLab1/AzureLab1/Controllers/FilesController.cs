using AzureTableAuthApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IAuthorizationService _authService;

    public FilesController(IAuthorizationService authService)
    {
        _authService = authService;
    }

//    [HttpPost("")]

//    [HttpPost("{name}")]

}