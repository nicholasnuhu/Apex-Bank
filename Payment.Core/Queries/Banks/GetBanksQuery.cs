using MediatR;
using Payment.Core.Interfaces;
using Payment.Core.Utilities.PaymentGatewaySettings;

namespace Payment.Core.Queries.Banks
{
    public class GetBanksQuery : IRequest<IEnumerable<BankResponseDto>>
    {

    }

    public class GetBanksQueryHandler : IRequestHandler<GetBanksQuery, IEnumerable<BankResponseDto>>
    {
        private readonly IBankService _bankService;

        public GetBanksQueryHandler(IBankService bankService)
        {
            _bankService = bankService;
        }

        public async Task<IEnumerable<BankResponseDto>> Handle(GetBanksQuery request, CancellationToken cancellationToken)
        {
            var result = await _bankService.GetAllBanksAsync();
            return result.Data;
        }
    }
}
