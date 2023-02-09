using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.DTOs.Account
{
    public class ConfirmAccountDTO
    {
        public int Code { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class ConfirmAccountDTOValidator : AbstractValidator<ConfirmAccountDTO> 
    {
        public ConfirmAccountDTOValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Confirmation code can't be empty");
            RuleFor(x => x.Password).MinimumLength(6).WithMessage("Password must be more than 6 characters long");
            RuleFor(x => x.ConfirmPassword).Equal(x=>x.Password).WithMessage("Passwords must be the same");
        }
    }
}
