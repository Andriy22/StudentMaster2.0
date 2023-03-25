using backend.BLL.Common.DTOs.Auth;
using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthorizationAsync(AuthorizationDTO model)
        {
            return Ok(await _authService.AuthorizationAsync(model));
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(RefreshTokenDTO model)
        {
            return Ok(await _authService.RefreshAsync(model));
        }
    }
}
