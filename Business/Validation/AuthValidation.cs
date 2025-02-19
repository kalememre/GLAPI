using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Infrastructure.DTO;

namespace Business.Validation
{
    public class AuthValidation : AbstractValidator<AuthRequest>
    {
        public AuthValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}