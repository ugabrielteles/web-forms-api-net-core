using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace OrderApi.ValueObjects
{
    public class CreateUserRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public ValidationResult Validate() => new CreateUserRequestValidation().Validate(this);

        class CreateUserRequestValidation : AbstractValidator<CreateUserRequest>
        {
            public CreateUserRequestValidation()
            {
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage(x => InvalidMessage(nameof(x.Name)))
                    .NotNull().WithMessage(x => RequiredMessage(nameof(x.Name)));

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage(x => InvalidMessage(nameof(x.Email)))
                    .NotNull().WithMessage(x => RequiredMessage(nameof(x.Email)));

                When(x => !string.IsNullOrEmpty(x.Email), () => 
                {
                    RuleFor(x => x.Email).EmailAddress();
                });

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage(x => InvalidMessage(nameof(x.Password)))
                    .NotNull().WithMessage(x => RequiredMessage(nameof(x.Password)));
            }

            private static string RequiredMessage(string property) => $"{property} is required";
            private static string InvalidMessage(string property) => $"{property} is invalid";
        }
    }
}