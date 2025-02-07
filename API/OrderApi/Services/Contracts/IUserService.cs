using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using OrderApi.ValueObjects;

namespace OrderApi.Services.Contracts
{
    public interface IUserService
    {
        Task<ValidationResult> CreateUser(CreateUserRequest request);
        Task<ValidationResult> UserIsValid(LoginRequest request);
    }
}