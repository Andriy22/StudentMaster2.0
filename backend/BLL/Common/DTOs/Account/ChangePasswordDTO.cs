using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.DTOs.Account
{
    public class ChangePasswordDTO
    {
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordDTOValidator : AbstractValidator<ChangePasswordDTO>
    {
        public ChangePasswordDTOValidator()
        {
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password must be more than 6 characters long");
            RuleFor(x => x.NewPassword).MinimumLength(6).WithMessage("Password must be more than 6 characters long");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.NewPassword).WithMessage("Passwords must be the same");
        }
    }
}
