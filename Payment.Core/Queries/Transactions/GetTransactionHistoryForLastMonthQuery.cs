using MediatR;
using Payment.Core.DTOs;
using Payment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Core.Queries.Transactions
{
    public class GetTransactionHistoryForLastMonthQuery : IRequest<PaginationResult<IEnumerable<TransactionHistoryResponseDto>>>
    {
        public string WalletId { get; set; }
        public int PageNumber { get; set; }
    }

    public class GetTransactionHistoryForLastMonthQueryHandler : IRequestHandler<GetTransactionHistoryForLastMonthQuery, PaginationResult<IEnumerable<TransactionHistoryResponseDto>>>
    {
        private readonly ITransactionService _transactionService;

        public GetTransactionHistoryForLastMonthQueryHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<PaginationResult<IEnumerable<TransactionHistoryResponseDto>>> Handle(GetTransactionHistoryForLastMonthQuery request, CancellationToken cancellationToken)
        {
            var result = await _transactionService.GetTransactionHistoryOfTheLastMonthAsync(request.WalletId, request.PageNumber);
            return result.Data;
        }
    }
}
