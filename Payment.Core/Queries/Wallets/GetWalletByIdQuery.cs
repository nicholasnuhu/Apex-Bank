using MediatR;
using Payment.Core.DTOs.WalletDtos;
using Payment.Core.Interfaces;
using Payment.Core.Utilities.PaymentGatewaySettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Core.Queries.Wallets
{
    
    public class GetWalletByIdQuery : IRequest<WalletResponseDto>
    {
        public string Id { get; set; }
    }

    public class GetWalletQueryHandler : IRequestHandler<GetWalletByIdQuery, WalletResponseDto>
    {
        private readonly IWalletService _walletService;

        public GetWalletQueryHandler(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public async Task<WalletResponseDto> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _walletService.GetWalletByIdAsync(request.Id);
            return result.Data;
        }
    }
}
