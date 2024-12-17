using MediatR;
using Microsoft.AspNetCore.Identity;
using Payment.Core.DTOs.UserDtos;
using Payment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Core.Commands.User
{
    public class RegisterUserCommand : IRequest<IdentityResult>
    {
        public UserForRegistrationDto UserForRegistration { get; set; }
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IdentityResult>
    {
        private readonly IAuthenticationService _authenticationService;

        public RegisterUserCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<IdentityResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            return await _authenticationService.RegisterUser(request.UserForRegistration);
        }
    }
}
