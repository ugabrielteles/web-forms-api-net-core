using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace OrderApi.ValueObjects
{
    public class CreateOrderRequest
    {
        public string? Description { get; set; }

        public decimal? Value { get; set; }

        public string? Street { get; set; }

        public string? ZipCode { get; set; }

        public string? Number { get; set; }

        public string? Neighborhood { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public ValidationResult Validate() => new CreateOrderRequestValidation().Validate(this);
        
        class CreateOrderRequestValidation : AbstractValidator<CreateOrderRequest> 
        {
            public CreateOrderRequestValidation()
            {
                RuleFor(x => x.Description).NotEmpty().NotNull();
                RuleFor(x => x.Value).GreaterThan(0);
                RuleFor(x => x.Street).NotEmpty().NotNull();
                RuleFor(x => x.ZipCode).NotEmpty().NotNull();
                RuleFor(x => x.Number).NotEmpty().NotNull();
                RuleFor(x => x.Neighborhood).NotEmpty().NotNull();
                RuleFor(x => x.State).NotEmpty().NotNull();
            }
        }
    }
}