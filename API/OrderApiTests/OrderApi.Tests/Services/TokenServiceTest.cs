using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using OrderApi.Services;
using OrderApi.Settings;

namespace OrderApi.Tests.Services
{
    public class TokenServiceTest
    {

        [Fact(DisplayName = "Cria token - sucesso")]
        public void Cria_Token_Sucesso()
        {
            // Arrange
            var settings = new AuthenticationSettings
            {
                SecretKey = "mysupersecretkey123456789103265565",
                Issuer = "testissuer",
                Audience = "testaudience",
                ExpirationMinutes = 30
            };

            var tokenService = new TokenService(settings);
            var username = "testuser";

            // Act
            var (token, expiration, tokenType) = tokenService.CreateToken(username);

            // Assert
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.Equal(username, jwtToken.Claims.First(c => c.Type == ClaimTypes.Name).Value);
            Assert.Equal("testissuer", jwtToken.Issuer);
            Assert.Equal("Bearer", tokenType);
            Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
        }

        [Fact(DisplayName = "Valida Token - Sucesso")]
        public void Valida_Token_Nulo()
        {
            // Arrange
            var settings = new AuthenticationSettings
            {
                SecretKey = "mysupersecretkey123456789103265565",
                Issuer = "testissuer",
                Audience = "testaudience",
                ExpirationMinutes = -1 // Expired token
            };

            var tokenService = new TokenService(settings);
            var username = "testuser";
            var (token, _, _) = tokenService.CreateToken(username);

            // Act
            var result = tokenService.ValidateToken(token);

            // Assert
            Assert.NotNull(result);
        }    
        

    }
}