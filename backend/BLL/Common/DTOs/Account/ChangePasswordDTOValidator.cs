using FluentValidation;

namespace backend.BLL.Common.DTOs.Account
{
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
