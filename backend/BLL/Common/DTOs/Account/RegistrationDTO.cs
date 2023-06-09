﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.BLL.Common.DTOs.Account
{
    public class RegistrationDTO
    {
        public string FistName { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class RegistrationDTOValidator: AbstractValidator<RegistrationDTO>
    {
        public RegistrationDTOValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
        }
    }
}
