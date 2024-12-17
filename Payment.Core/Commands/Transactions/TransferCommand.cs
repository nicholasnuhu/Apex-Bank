using MediatR;
using Payment.Core.DTOs.PaystackDtos.FundAccountDto;
using Payment.Core.DTOs.TransactionsDto;
using Payment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Core.Commands.Transactions
{
    public class TransferCommand : IRequest<string>
    {
        public TransferRequestDto TransferRequest { get; set; }
        public string WalletId { get; set; }
    }

    public class TransferCommandHandler : IRequestHandler<TransferCommand, string>
    {
        private readonly ITransactionService _transactionService;
        public TransferCommandHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<string> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            var result = await _transactionService.Transfer(request.TransferRequest, request.WalletId);
            return result.Data;
        }
    }
}
