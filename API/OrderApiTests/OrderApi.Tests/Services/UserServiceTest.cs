using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using OrderApi.Models;
using OrderApi.Repositories.Contracts;
using OrderApi.Services;
using OrderApi.ValueObjects;

namespace OrderApi.Tests.Services
{
    public class UserServiceTest
    {
        [Fact(DisplayName = "Cria um usuario com sucesso")]
        public async Task Cria_Usuario_Sucesso()
        {
            // Arrange
            var logger = Mock.Of<ILogger<UserService>>();
            var userRepository = new Mock<IUserRepository>();
            var userService = new UserService(logger, userRepository.Object);

            var request = new CreateUserRequest
            {
                Name = "Test User",
                Email = "test@email.com",
                Password = "password123"
            };

            userRepository.Setup(x => x.Search(It.IsAny<Expression<Func<User, bool>>>()))
                .ReturnsAsync((User)null!);

            userRepository.Setup(x => x.HashBytesAsync(It.IsAny<string>()))
                .ReturnsAsync("HASHPASSWORD");

            userRepository.Setup(x => x.Add(It.IsAny<User>()))
                .ReturnsAsync(new User());

            // Act
            var result = await userService.CreateUser(request);

            // Assert
            Assert.True(result.IsValid);
            userRepository.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
        }

        [Fact(DisplayName = "Cria usuario request nulo")]
        public async Task Cria_Usuario_Request_Nulo()
        {
            // Arrange
            var logger = Mock.Of<ILogger<UserService>>();
            var userRepository = Mock.Of<IUserRepository>();
            var userService = new UserService(logger, userRepository);

            CreateUserRequest request = null!;

            // Act
            var result = await userService.CreateUser(request);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == "Request não foi informado!");
        }     


    }
}