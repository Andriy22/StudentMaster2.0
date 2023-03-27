using FluentValidation;

namespace backend.BLL.Common.DTOs.Account;

public class RegistrationDTOValidator : AbstractValidator<RegistrationDTO>
{
    public RegistrationDTOValidator()
    {
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
    }
}