using AzureTableAuthApi.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthorizationService _authService;

    public AuthController(IAuthorizationService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Login) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Login & Password are mandatory");
        }

        var sessionId = await _authService.LoginAsync(request.Login, request.Password);

        if (sessionId == null)
        {
            return Unauthorized("Wrong login and/or password");
        }

        return Ok(new
        {
            sessionId
        });
    }

    [HttpDelete("logout/{login}")]
    public async Task<IActionResult> Logout(string login)
    {
        if (string.IsNullOrWhiteSpace(login))
            return BadRequest("Login is needed to log out");

        var result = await _authService.LogoutAsync(login);

        if (!result)
            return NotFound("Session not found");

        return Ok(new
        {
            message = $"Account {login} logged out successfully",login
        });
    }
}