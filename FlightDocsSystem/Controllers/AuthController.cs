using Microsoft.AspNetCore.Mvc;
using FlightDocsSystem.Interfaces;
using FlightDocsSystem.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace FlightDocsSystem.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
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

        // POST: api/Auth/Login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var token = await _authService.LoginAsync(loginDto.Email, loginDto.Password);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDto logoutDto)
        {
            await _authService.LogoutAsync(logoutDto.SessionToken);
            return Ok(new { Message = "Successfully logged out." });
        }

        [Authorize(Policy = "ActiveToken")]
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
        }

        // POST: api/Auth/TerminateSession
        [Authorize(Policy = "ActiveToken")]
        [HttpPost("TerminateSession")]
        public async Task<IActionResult> TerminateSession(int userId)
        {
            await _authService.ChangeActiveSessionAsync(userId);
            return Ok(new { Message = "Session terminated successfully." });
        }
    }
}
