using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.Core.Commands.Transactions;
using Payment.Core.DTOs.TransactionsDto;
using Payment.Core.Interfaces;
using Payment.Core.Queries.Transactions;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    [ApiController]

    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ISender _sender;
        public TransactionsController(ITransactionService transactionService, ISender sender)
        {
            _transactionService = transactionService;
            _sender = sender;
        }

        [HttpGet("transaction-history/{walletId}")]
        public async Task<IActionResult> GetTransactionHistoryAsync([FromRoute] string walletId, int pageNumber)
        {
            var transactionHistory = await _sender.Send(new GetTransactionHistoryQuery()
            {
                PageNumber = pageNumber,
                WalletId = walletId
            });
            return Ok(transactionHistory);
        }

        [HttpGet("last-month-transaction-history/{walletId}")]
        public async Task<IActionResult> GetLastMonthTransactionHistoryAsync([FromRoute] string walletId, int pageNumber)
        {
            var transactionHistory = await _sender.Send(new GetTransactionHistoryForLastMonthQuery()
            {
                PageNumber = pageNumber,
                WalletId = walletId
            });
            return Ok(transactionHistory);
        }

        [HttpPost("bank-transfer/{walletId}")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequestDto request, [FromRoute] string walletId)
        {
            var result = await _sender.Send(new TransferCommand()
            {
                TransferRequest = request,
                WalletId = walletId
            });
            return Ok(result);
        }

        [HttpPost("local-transfer/{walletId}")]
        public async Task<IActionResult> LocalTransfer([FromRoute] string walletId, [FromBody] LocalTransferRequestDto request)
        {
            var result = await _sender.Send(new LocalTransferCommand()
            {
                TransferRequest = request,
                WalletId = walletId
            });
            return Ok(result);
        }
    }
}
