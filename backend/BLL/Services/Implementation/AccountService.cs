using backend.BLL.Common.DTOs.Account;
using backend.BLL.Common.Exceptions;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
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

        public AccountService(UserManager<User> userManager)
        {
            _userManager = userManager;
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
    }
}
