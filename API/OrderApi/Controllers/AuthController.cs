using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Services.Contracts;
using OrderApi.ValueObjects;

namespace OrderApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _userService.UserIsValid(request);

            if(!response.IsValid)
            {
                return Unauthorized();
            }

            var responseToken = _tokenService.CreateToken(request.Email!);

            return Ok(new 
            { 
                access_token = responseToken.Token,
                expires_in = responseToken.Expiration,
                token_type = responseToken.TokenType
            });
        }      

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var response = await _userService.CreateUser(request);

            if(!response.IsValid)
                return BadRequest(response.Errors);

            return Ok();
        }
    }
}