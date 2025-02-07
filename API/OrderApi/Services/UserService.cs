using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using OrderApi.Repositories.Contracts;
using OrderApi.Services.Contracts;
using OrderApi.ValueObjects;

namespace OrderApi.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;

        public UserService(ILogger<UserService> logger, IUserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<ValidationResult> CreateUser(CreateUserRequest request)
        {
            var result = new ValidationResult();

            try
            {
                if (request == null) throw new ArgumentNullException(nameof(request));

                result = request.Validate();

                if (!result.IsValid)
                {
                    return result;
                }

                if((await _userRepository.Search(x => x.Email == request.Email)) != null)
                {
                    result.Errors.Add(new ValidationFailure("Email", "Ja existe um usuario para o e-mail informado!"));
                    return result;
                }

                var createUserResult = await _userRepository.Add(new()
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = await _userRepository.HashBytesAsync(request.Password!),
                    CreateAt = DateTime.Now
                });

                return result;
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception, exception.Message);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return result;
            }
        }

        public async Task<ValidationResult> UserIsValid(LoginRequest request)
        {
            var result = new ValidationResult();

            try
            {
                if (request == null) throw new ArgumentNullException(nameof(request));

                result = request.Validate();

                if (!result.IsValid)
                {
                    return result;
                }

                var passwordHash = await _userRepository.HashBytesAsync(request.Password!);

                if((await _userRepository.Search(x => x.Email == request.Email && x.Password == passwordHash)) == null)
                {
                    result.Errors.Add(new ValidationFailure("Auntenticacao", "Usuario e senha informado são invalidos!"));
                    return result;
                }               

                return result;
            }
            catch (ArgumentNullException exception)
            {
                _logger.LogError(exception, exception.Message);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return result;
            }
        }
    }
}