using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.Core.Commands.Wallets;
using Payment.Core.DTOs.PaystackDtos.FundAccountDto;
using Payment.Core.DTOs.WalletDtos;
using Payment.Core.Interfaces;
using Payment.Core.Queries.Wallets;
using System.Net.Mime;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Customer")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly ISender _sender;

        public WalletsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _sender.Send(new GetWalletByIdQuery()
            {
                Id = id
            });
            return Ok(result);
        }

        [HttpGet("user-wallet/{userId}")]
        public async Task<IActionResult> GetUserWallet(string userId)
        {
            //var result = await _walletService.GetWalletByUserIdAsync(userId);
            var result = await _sender.Send(new GetWalletByUserIdQuery()
            {
                UserId = userId
            });
            return Ok(result);
        }

        [HttpPost("create-wallet")]
        public async Task<IActionResult> CreateWallet(CreateWalletRequest walletRequestDto)
        {
            var result = await _sender.Send(new CreateWalletCommand()
            {
                CreateWalletRequest = walletRequestDto
            });
            return Ok(result);
        }


        [HttpPost("initialize-fund-wallet/{walletId}")]
        public async Task<IActionResult> InitializationFundWallet([FromBody] FundWalletRequest request, [FromRoute] string walletId)
        {
            var result = await _sender.Send(new InitializationFundWalletCommand()
            {
                FundWalletRequest = request,
                WalletId = walletId
            });
            return Ok(result);
        }


        [HttpPost("fund-wallet-verification/{walletId}")]
        public async Task<IActionResult> FundWallets([FromBody] VerifyTransactionRequestDto reference, [FromRoute] string walletId)
        {
            var result = await _sender.Send(new FundWalletCommand()
            {
                VerifyTransaction = reference,
                WalletId = walletId
            });
            return Ok(result);
        }

    }
}
