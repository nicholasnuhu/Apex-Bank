using MediatR;
using Payment.Core.DTOs.UserDtos;
using Payment.Core.Interfaces;

namespace Payment.Core.Commands.User
{
    public class ValidateUserCommand : IRequest<string>
    {
        public UserForAuthenticationDto UserForAuthentication { get; set; }
    }

    public class ValidateUserCommandHandler : IRequestHandler<ValidateUserCommand, string>
    {
        private readonly IAuthenticationService _authenticationService;

        public ValidateUserCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        public async Task<string> Handle(ValidateUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.ValidateUser(request.UserForAuthentication);
            if (!result)
            {
                return "FailedAuthorization";
            }
            return await _authenticationService.CreateToken();
        }
    }
}
