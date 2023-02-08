using backend.BLL.Common.DTOs.Account;
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
        Task<int> GenerateConfirmationCodeAsync(string email);
    }
}
