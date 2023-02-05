using backend.BLL.Common.DTOs.Auth;
using backend.BLL.Common.VMs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthorizationVM> AuthorizationAsync(AuthorizationDTO model);
        Task<AuthorizationVM> RefreshAsync(RefreshTokenDTO model);
    }
}
