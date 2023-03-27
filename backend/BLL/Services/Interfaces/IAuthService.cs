using backend.BLL.Common.DTOs.Auth;
using backend.BLL.Common.VMs.Auth;

namespace backend.BLL.Services.Interfaces;

public interface IAuthService
{
    Task<AuthorizationVM> AuthorizationAsync(AuthorizationDTO model);
    Task<AuthorizationVM> RefreshAsync(RefreshTokenDTO model);
}