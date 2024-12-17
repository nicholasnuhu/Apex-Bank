using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payment.Core.Commands.Banks;
using Payment.Core.DTOs.BeneficiaryDto;
using Payment.Core.DTOs.PaystackDtos.BankDtos;
using Payment.Core.Interfaces;
using Payment.Core.Queries.Banks;
using System.Net;
using System.Net.Mime;

namespace Payment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BanksController : ControllerBase
    {
        private readonly ISender _sender;
        public BanksController(ISender sender)
        {
            _sender = sender;
        }
        [HttpGet("get-all-banks")]
        public async Task<IActionResult> GetAllBanks()
        {
            //var result = await _bankService.GetAllBanksAsync();
            var result = await _sender.Send(new GetBanksQuery());
            return Ok(result);
        }

        [HttpPost("verify-account-number")]
        public async Task<IActionResult> VerifyAccountNumber([FromBody] VerifyAccountRequestDto request)
        {
            var result = await _sender.Send(new VerifyAccountNumberCommand()
            {
                VerifyAccountRequest = request,
            });
            return Ok(result);
        }

    }
}
