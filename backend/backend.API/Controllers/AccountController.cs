using backend.BLL.Common.DTOs.Account;
using backend.BLL.Common.VMs.Email;
using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IEmailService _emailService;
    private readonly IRazorRenderService _razorRenderService;

    public AccountController(IRazorRenderService razorRenderService, IEmailService emailService,
        IAccountService accountService)
    {
        _razorRenderService = razorRenderService;
        _emailService = emailService;
        _accountService = accountService;
    }

    [HttpPost("confirm-account")]
    public async Task<IActionResult> ConfirmAccountAsync(ConfirmAccountDTO model)
    {
        await _accountService.ConfirmAccountAsync(model);

        return Ok();
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDTO model)
    {
        await _accountService.ChangePasswordAsync(model, User.Identity.Name);
        return Ok();
    }

    [Authorize]
    [HttpPost("change-avatar")]
    public async Task<IActionResult> ChangeAvatarAsync([FromForm] IFormFile file)
    {
        await _accountService.ChangeAvatarAsync(file, User.Identity.Name);
        return Ok();
    }

    [Authorize]
    [HttpGet("get-avatar")]
    public async Task<IActionResult> GetAvatarAsync()
    {
        return Ok(await _accountService.GetAvatar(User.Identity.Name));
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetYourselfInfoAsync()
    {
        return Ok(await _accountService.GetAccountInfo(User.Identity.Name));
    }

    [HttpGet("check-account-confirmation/{email}")]
    public async Task<IActionResult> CheckAccountInformationAsync(string email)
    {
        Thread.Sleep(2000);
        return Ok(await _accountService.IsAccountConfirmedAsync(email));
    }

    [HttpPost("create-account")]
    public async Task<IActionResult> CreateAccountAsync(RegistrationDTO model)
    {
        await _accountService.CreateAccountAsync(model);

        var code = await _accountService.GenerateConfirmationCodeAsync(model.Email);

        await _emailService.SendEmailAsync(model.Email, "StudentMaster Account Confirmation",
            await _razorRenderService.RenderEmailConfirmationAsync(new ConfirmCodeVM
            {
                Code = code
            }));

        return Ok();
    }
}