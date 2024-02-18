using backend.BLL.Common.Consts;
using backend.BLL.Common.DTOs.Account;
using backend.BLL.Common.Exceptions;
using backend.BLL.Common.VMs.Account;
using backend.BLL.Services.Interfaces;
using backend.DAL.Entities;
using backend.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.BLL.Services.Implementation;

public class AccountService : IAccountService
{
    private readonly IRepository<ConfirmCode> _confirmCodeRepository;
    private readonly IFileService _fileService;
    private readonly IRandomService _randromService;
    private readonly UserManager<User> _userManager;
    private readonly IRepository<User> _userRepository;

    public AccountService(UserManager<User> userManager, IRepository<ConfirmCode> confirmCodeRepository,
        IRandomService randromService, IRepository<User> userRepository, IFileService fileService)
    {
        _userManager = userManager;
        _confirmCodeRepository = confirmCodeRepository;
        _randromService = randromService;
        _userRepository = userRepository;
        _fileService = fileService;
    }

    public async Task<string> ChangeAvatarAsync(IFormFile file, string uid)
    {
        var user = await _userManager.FindByIdAsync(uid);

        if (user == null) throw new CustomHttpException("User is invalid");

        var path = await _fileService.SaveFile(file, FileConstants.AvatarFolder);

        user.Img = new Attachment
        {
            Path = path
        };

        _userRepository.Edit(user);

        return path;
    }

    public async Task ChangePasswordAsync(ChangePasswordDTO model, string uid)
    {
        var user = await _userManager.FindByIdAsync(uid);

        if (user == null) throw new CustomHttpException("User is invalid");

        var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

        if (!result.Succeeded) throw new CustomHttpException($"Failed to change password: {result}");
    }

    public async Task ConfirmAccountAsync(ConfirmAccountDTO model)
    {
        var code = await _confirmCodeRepository.GetQueryable(x => x.Code == model.Code && x.IsUsed == false)
            .Include(x => x.user).FirstOrDefaultAsync();

        if (code == null) throw new CustomHttpException("Code is invalid");

        var user = code.user;

        if (user == null) throw new CustomHttpException("User is invalid");
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
            FirstName = model.FirstName,
            LastName = model.LastName,
            Name = model.Name,
            Created = DateTime.UtcNow,
            EmailConfirmed = true,
            Img = new Attachment
            {
                Path = FileConstants.DefaultAvatar
            }
        };

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded) throw new CustomHttpException(result.Errors.FirstOrDefault().Description);

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

        if (user == null) throw new CustomHttpException("User not found");

        var code = new ConfirmCode
        {
            Code = _randromService.GetRandomNumber(10000000, 99999999),
            UserID = user.Id,
            IsUsed = false,
            CreationTime = DateTime.UtcNow
        };

        _confirmCodeRepository.Add(code);

        return code.Code;
    }

    public async Task<AccountInfoVM> GetAccountInfo(string uid)
    {
        var user = await _userRepository.GetQueryable(x => x.Id.ToLower() == uid.ToLower()).Include(x => x.Img)
            .FirstOrDefaultAsync();

        if (user == null) throw new CustomHttpException("User not found");

        return new AccountInfoVM
        {
            Avatar = user.Img.Path,
            Email = user.Email,
            Id = uid,
            Name = $"{user.FirstName} {user.Name} {user.LastName}",
            Roles = (await _userManager.GetRolesAsync(user)).ToList()
        };
    }

    public async Task<string> GetAvatar(string uid)
    {
        var user = await _userRepository.GetQueryable(x => x.Id.ToLower() == uid.ToLower()).Include(x => x.Img)
            .FirstOrDefaultAsync();

        if (user == null) throw new CustomHttpException("User not found");

        return user.Img.Path;
    }

    public async Task<bool> IsAccountConfirmedAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null) throw new CustomHttpException("User not found");

        return user.EmailConfirmed;
    }
}