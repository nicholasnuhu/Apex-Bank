using MediatR;
using Payment.Core.DTOs.TransactionsDto;
using Payment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Core.Commands.Transactions
{
    public class LocalTransferCommand : IRequest<string>
    {
        public LocalTransferRequestDto TransferRequest { get; set; }
        public string WalletId { get; set; }
    }

    public class LocalTransferCommandHandler : IRequestHandler<LocalTransferCommand, string>
    {
        private readonly IWalletService _walletService;
        public LocalTransferCommandHandler(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public async Task<string> Handle(LocalTransferCommand request, CancellationToken cancellationToken)
        {
            var result = await _walletService.LocalTransferAsync(request.WalletId, request.TransferRequest);
            return result.Data;
        }
    }
}
