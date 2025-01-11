using FlightDocsSystem.Dtos;
using FlightDocsSystem.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _context;

    public AuthController(IAuthService authService, ITokenService tokenService, IHttpContextAccessor context)
    {
        _authService = authService;
        _tokenService = tokenService;
        _context = context;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
        }
    }
    [Authorize(Policy = "AllowAll")]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout([FromBody] string token)
    {
        try
        {
            await _authService.LogoutAsync(token);
            return Ok(new { Message = "Successfully logged out." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while processing your request.", Details = ex.Message });
        }
    }
    [Authorize(Policy = "AllowAll")]
    [HttpPost("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        try
        {
            var oldToken = _tokenService.GetTokenFromHTTPContext(_context);
            var newToken = await _authService.RefreshTokenAsync(oldToken);
            return Ok(new { Token = newToken });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while refreshing the token.", Details = ex.Message });
        }
    }
    [Authorize(Policy = "AdminOnly")]
    [HttpPost("ChangeSessionStatus")]
    public async Task<IActionResult> TerminateSession(string userId)
    {
        try
        {
            await _authService.ChangeActiveSessionAsync(userId);
            return Ok(new { Message = "Session change successfully." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while terminating the session.", Details = ex.Message });
        }
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost("ChangeOwner")]
    public async Task<IActionResult> ChangeOwner([FromQuery] string currentOwner, [FromQuery] string newOwner)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.ChangeOwner(currentOwner, newOwner);
            return Ok(new { Message = "Owner changed successfully." });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while changing the owner.", Details = ex.Message });
        }
    }

}
