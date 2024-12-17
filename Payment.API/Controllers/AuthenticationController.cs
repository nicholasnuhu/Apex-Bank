using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment.Core.Commands.User;
using Payment.Core.DTOs.UserDtos;
using Payment.Domain.Models;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender _sender;
        private const string Fail = "FailedAuthorization";

        public AuthenticationController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var result = await _sender.Send(new RegisterUserCommand { UserForRegistration = userForRegistration });
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            var result = await _sender.Send(new  ValidateUserCommand{ UserForAuthentication = userForAuthentication });
            if (result == Fail) return Unauthorized();
            return Ok(new { token = result });
        }
    }
    }
