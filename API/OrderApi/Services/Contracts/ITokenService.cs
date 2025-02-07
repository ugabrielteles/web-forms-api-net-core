using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OrderApi.Services.Contracts
{
    public interface ITokenService
    {
        (string Token, long Expiration, string TokenType) CreateToken(string username);
        ClaimsPrincipal? ValidateToken(string token);
    }
}