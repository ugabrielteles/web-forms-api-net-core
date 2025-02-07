using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace OrderApi.ValueObjects
{
    public class LoginRequest
    {
        public string? Email { get; set; }
        public string? Password { get; set; }

        public ValidationResult Validate() => new LoginRequestValidation().Validate(this);

        class LoginRequestValidation : AbstractValidator<LoginRequest>
        {
            public LoginRequestValidation()
            {
                RuleFor(x => x.Email).NotEmpty().NotNull().WithMessage("O Email não foi informado!");
                RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("A senha não foi informada!");

                When(x => string.IsNullOrEmpty(x.Email), () => 
                {
                    RuleFor(x => x.Email).EmailAddress();
                });
            }
        }
    }
}