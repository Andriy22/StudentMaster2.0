using backend.BLL.Common.DTOs.Account;
using backend.BLL.Common.Exceptions;
using backend.BLL.Common.VMs.Email;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<ConfirmCode> _confirmCodeRepository;

        public AccountService(UserManager<User> userManager, IRepository<ConfirmCode> confirmCodeRepository)
        {
            _userManager = userManager;
            _confirmCodeRepository = confirmCodeRepository;
        }

        public async Task CreateAccountAsync(RegistrationDTO model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FistName,
                LastName = model.LastName,
                Name = model.Name,
                Created = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, "5358254AaV");

            if (!result.Succeeded)
            {
                throw new CustomHttpException(result.Errors.FirstOrDefault().Description);
            }

            await _userManager.AddToRoleAsync(user, model.Role);

            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new CustomHttpException(result.Errors.FirstOrDefault().Description);
            }
        }

        public async Task<int> GenerateConfirmationCodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new CustomHttpException("User not found");
            }

            var code = new ConfirmCode
            {
                Code = 1111,
                UserID = user.Id,
                IsUsed = false,
                CreationTime = DateTime.UtcNow,
            };

            _confirmCodeRepository.Add(code);

            return code.Code;
        }
    }
}
