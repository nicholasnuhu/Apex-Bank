using MediatR;
using Payment.Core.DTOs;
using Payment.Core.Interfaces;
using Payment.Domain.Models;

namespace Payment.Core.Queries.Transactions
{
    public class GetTransactionHistoryQuery : IRequest<PaginationResult<IEnumerable<TransactionHistoryResponseDto>>>
    {
        public string WalletId { get; set; }
        public int PageNumber { get; set; }
    }

    public class GetTransactionHistoryQueryHandler : IRequestHandler<GetTransactionHistoryQuery, PaginationResult<IEnumerable<TransactionHistoryResponseDto>>>
    {
        private readonly ITransactionService _transactionService;

        public GetTransactionHistoryQueryHandler(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<PaginationResult<IEnumerable<TransactionHistoryResponseDto>>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            var result = await _transactionService.GetTransactionHistoryAsync(request.WalletId, request.PageNumber);
            return result.Data;
        }
    }
}
