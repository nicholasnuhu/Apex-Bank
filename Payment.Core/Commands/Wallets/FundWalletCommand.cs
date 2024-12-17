using MediatR;
using Payment.Core.DTOs.PaystackDtos.FundAccountDto;
using Payment.Core.Interfaces;
using Paystack.Net.SDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Core.Commands.Wallets
{
    public class FundWalletCommand : IRequest<string>
    {
        public VerifyTransactionRequestDto VerifyTransaction { get; set; }
        public string WalletId { get; set; }
    }

    public class FundWalletCommandHandler : IRequestHandler<FundWalletCommand, string>
    {
        private readonly IWalletService _walletService;
        public FundWalletCommandHandler(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public async Task<string> Handle(FundWalletCommand request, CancellationToken cancellationToken)
        {
            var result = await _walletService.FundWalletVerificationAsync(request.VerifyTransaction, request.WalletId);
            return result.Data;
        }
    }
}
