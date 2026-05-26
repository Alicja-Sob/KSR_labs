using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/users")]
public class RegisterController : ControllerBase
{
    private readonly IUserService _userService;

    public RegisterController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Login) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Login and password are required.");
        }

        var exists = await _userService.UserExistsAsync(request.Login);

        if (exists)
        {
            return Conflict("User already exists.");
        }

        await _userService.CreateUserAsync(request.Login, request.Password);

        return Ok(new
        {
            message = "User registered successfully",
            login = request.Login
        });
    }
}