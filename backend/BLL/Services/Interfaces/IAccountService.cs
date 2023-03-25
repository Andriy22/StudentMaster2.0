using backend.BLL.Common.DTOs.Account;
using backend.BLL.Common.VMs.Account;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Interfaces
{
    public interface IAccountService
    {
        Task CreateAccountAsync(RegistrationDTO model);
        Task ConfirmAccountAsync(ConfirmAccountDTO model);
        Task<int> GenerateConfirmationCodeAsync(string email);
        Task<bool> IsAccountConfirmedAsync(string email);
        Task ChangePasswordAsync(ChangePasswordDTO model, string uid);
        Task<string> ChangeAvatarAsync(IFormFile file, string uid);
        Task<string> GetAvatar(string uid);

        Task<AccountInfoVM> GetAccountInfo(string uid);
    }
}
