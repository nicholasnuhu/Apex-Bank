using MediatR;
using Payment.Core.DTOs.WalletDtos;
using Payment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Core.Queries.Wallets
{
    public class GetWalletByUserIdQuery : IRequest<WalletResponseDto>
    {
        public string UserId { get; set; }
    }

    public class GetWalletByUserIdQueryHandler : IRequestHandler<GetWalletByUserIdQuery, WalletResponseDto>
    {
        private readonly IWalletService _walletService;

        public GetWalletByUserIdQueryHandler(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public async Task<WalletResponseDto> Handle(GetWalletByUserIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _walletService.GetWalletByUserIdAsync(request.UserId);
            return result.Data;
        }
    }
}
