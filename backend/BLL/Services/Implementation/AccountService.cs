using backend.BLL.Common.DTOs.Account;
using backend.BLL.Common.Exceptions;
using backend.BLL.Common.VMs.Email;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRandomService _randromService;
        private readonly IRepository<User> _userRepository;

        public AccountService(UserManager<User> userManager, IRepository<ConfirmCode> confirmCodeRepository, IRandomService randromService, IRepository<User> userRepository)
        {
            _userManager = userManager;
            _confirmCodeRepository = confirmCodeRepository;
            _randromService = randromService;
            _userRepository = userRepository;
        }

        public async Task ConfirmAccountAsync(ConfirmAccountDTO model)
        {
            var code = (await _confirmCodeRepository.GetQueryable(x => x.Code == model.Code && x.IsUsed == false).Include(x => x.user).FirstOrDefaultAsync());

            if (code == null)
            {
                throw new CustomHttpException("Code is invalid");
            }

            var user = code.user;

            if (user == null)
            {
                throw new CustomHttpException("User is invalid");
            }
            user.EmailConfirmed = true;

            var result = await _userManager.AddPasswordAsync(user, model.Password);

            if (result.Succeeded)
            {
                _userRepository.Edit(user);
                code.IsUsed = true;
                _confirmCodeRepository.Edit(code);
                return;
            }

            throw new CustomHttpException("We couldn't save your account!");
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

            var result = await _userManager.CreateAsync(user);

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
                Code = _randromService.GetRandomNumber(10000000, 99999999),
                UserID = user.Id,
                IsUsed = false,
                CreationTime = DateTime.UtcNow,
            };

            _confirmCodeRepository.Add(code);

            return code.Code;
        }
    }
}
