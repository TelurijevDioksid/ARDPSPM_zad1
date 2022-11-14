using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Span.Culturio.Api.Models.Auth;
using Span.Culturio.Api.Models.User;
using Span.Culturio.Api.Services;

namespace Span.Culturio.Api.Controllers
{
    [Route("/auth")]
    [ApiController]
    [Tags("Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IValidator<RegisterUserDto> _validator;

        public AuthController(IUserService userService, IValidator<RegisterUserDto> validator)
        {
            _userService = userService;
            _validator = validator;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> RegisterUser(RegisterUserDto registerUserDto)
        {
            ValidationResult result = _validator.Validate(registerUserDto);
            if (!result.IsValid) return BadRequest("ValidationError");

            var user = await _userService.RegisterUser(registerUserDto);
            if (user is null) return BadRequest();

            return Ok("Successful response");
        }

        /// <summary>
        /// Login
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<TokenDto>> Login(LoginDto loginUserDto)
        {
            var token = await _userService.LoginUser(loginUserDto);
            if (token is null) 
                return BadRequest("Bad username or password");

            return Ok(token);
        }
    }
}
