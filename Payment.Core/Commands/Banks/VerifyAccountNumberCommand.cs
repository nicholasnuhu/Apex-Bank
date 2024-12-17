using MediatR;
using Payment.Core.DTOs.PaystackDtos.BankDtos;
using Payment.Core.DTOs.TransactionsDto;
using Payment.Core.Interfaces;

namespace Payment.Core.Commands.Banks
{
    public class VerifyAccountNumberCommand : IRequest<VerifyAccountResponseDto>
    {
        public VerifyAccountRequestDto VerifyAccountRequest { get; set; }
    }

    public class VerifyAccountNumberCommandHandler : IRequestHandler<VerifyAccountNumberCommand, VerifyAccountResponseDto>
    {
        private readonly IBankAccountService _bankAccountService;
        public VerifyAccountNumberCommandHandler(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        public async Task<VerifyAccountResponseDto> Handle(VerifyAccountNumberCommand request, CancellationToken cancellationToken)
        {
            var result = await _bankAccountService.VerifyAccountNumber(request.VerifyAccountRequest);
            return result.Data;
        }
    }
}
