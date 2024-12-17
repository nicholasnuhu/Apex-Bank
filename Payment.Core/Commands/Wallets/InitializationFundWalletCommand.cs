using MediatR;
using Payment.Core.DTOs.PaystackDtos.FundAccountDto;
using Payment.Core.Interfaces;
using Paystack.Net.SDK.Models;

namespace Payment.Core.Commands.Wallets
{
    public class InitializationFundWalletCommand : IRequest<PaymentInitalizationResponseModel>
    {
        public FundWalletRequest FundWalletRequest { get; set; }
        public string WalletId { get; set; }
    }

    public class InitializationFundWalletCommandHandler : IRequestHandler<InitializationFundWalletCommand, PaymentInitalizationResponseModel>
    {
        private readonly IWalletService _walletService;
        public InitializationFundWalletCommandHandler(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public async Task<PaymentInitalizationResponseModel> Handle(InitializationFundWalletCommand request, CancellationToken cancellationToken)
        {
            var result = await _walletService.InitializeFundWallet(request.FundWalletRequest, request.WalletId);
            return result.Data;
        }
    }
}
