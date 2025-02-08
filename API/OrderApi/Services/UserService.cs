using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using OrderApi.Exceptions;
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

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ValidationResult> CreateUser(CreateUserRequest request)
        {
            var result = new ValidationResult();

            try
            {
                if (request == null) 
                    throw new DomainException("Request não foi informado!");

                result = request.Validate();

                if (!result.IsValid)
                {
                    return result;
                }

                //Verifica se já existe um usuário com o e-mail informado
                if ((await _userRepository.Search(x => x.Email == request.Email)) != null)
                {
                    result.Errors.Add(new ValidationFailure("Email", "Ja existe um usuario para o e-mail informado!"));
                    return result;
                }

                var createUserResult = await _userRepository.Add(new()
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = await _userRepository.HashBytesAsync(request.Password!), //Hash da senha para segurança da informação
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

        /// <summary>
        /// Valida se o usuario existe baseado no email e senha
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ValidationResult> UserIsValid(LoginRequest request)
        {
            _logger.LogDebug("Validação do usuario e senha iniciada");

            var result = new ValidationResult();

            try
            {
                if (request == null)
                    throw new DomainException(nameof(request));

                result = request.Validate();

                if (!result.IsValid)
                {
                    return result;
                }

                //Gera uma hash baseado na informação solicitada
                var passwordHash = await _userRepository.HashBytesAsync(request.Password!);

                //Por segurança compara as duas hash a salva na base = gerada com a informação que chega na requisição
                if ((await _userRepository.Search(x => x.Email == request.Email && x.Password == passwordHash)) == null)
                {
                    throw new DomainException("Usuario e senha informado são invalidos!");
                }

                _logger.LogDebug("Validação do usuario e senha finalizada com erro");

                return result;
            }
            catch (DomainException exception)
            {
                _logger.LogError(exception, exception.Message);
                _logger.LogDebug("Validação do usuario e senha finalizada com erro");
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, exception.Message);
                _logger.LogDebug("Validação do usuario e senha finalizada com erro");
                result.Errors.Add(new ValidationFailure(propertyName: exception.Message, errorMessage: exception.Message));
                return result;
            }
        }
    }
}