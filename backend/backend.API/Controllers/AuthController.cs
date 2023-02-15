using backend.BLL.Common.DTOs.Auth;
using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
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

        [HttpPost("authorization")]
        public async Task<IActionResult> AuthorizationAsync(AuthorizationDTO model)
        {
            Thread.Sleep(2000);
            return Ok(await _authService.AuthorizationAsync(model));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(RefreshTokenDTO model)
        {
            return Ok(await _authService.RefreshAsync(model));
        }
    }
}
