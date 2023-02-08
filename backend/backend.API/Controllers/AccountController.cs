using backend.BLL.Common.DTOs.Account;
using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System;
using backend.BLL.Common.VMs.Email;

namespace backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IRazorRenderService _razorRenderService;
        private readonly IEmailService _emailService;
        private readonly IAccountService _accountService;

        public AccountController(IRazorRenderService razorRenderService, IEmailService emailService, IAccountService accountService)
        {
            _razorRenderService = razorRenderService;
            _emailService = emailService;
            _accountService = accountService;
        }

        [HttpPost("create-account")]
        public async Task<IActionResult> CreateAccountAsync(RegistrationDTO model)
        {
            await _accountService.CreateAccountAsync(model);

            var code = await _accountService.GenerateConfirmationCodeAsync(model.Email);

            await _emailService.SendEmailAsync(model.Email, "WoT-STATS Email Confirmation", await _razorRenderService.RenderEmailConfirmationAsync(new ConfirmCodeVM
            {
                Code = code,
            }));

            return Ok();
        }
    }
}
