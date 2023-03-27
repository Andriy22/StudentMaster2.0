using FluentValidation;

namespace backend.BLL.Common.DTOs.Account;

public class ConfirmAccountDTOValidator : AbstractValidator<ConfirmAccountDTO>
{
    public ConfirmAccountDTOValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("Confirmation code can't be empty");
        RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password must be more than 6 characters long");
        RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords must be the same");
    }
}