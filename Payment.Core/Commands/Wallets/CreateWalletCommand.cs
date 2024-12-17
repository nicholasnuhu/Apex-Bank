using MediatR;
using Payment.Core.DTOs.WalletDtos;
using Payment.Core.Interfaces;

namespace Payment.Core.Commands.Wallets
{
    public class CreateWalletCommand : IRequest<CreateWalletResponse>
    {
        public CreateWalletRequest CreateWalletRequest { get; set; }
    }

    public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, CreateWalletResponse>
    {
        private readonly IWalletService _walletService;
        public CreateWalletCommandHandler(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public async Task<CreateWalletResponse> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            var result = await _walletService.CreateWalletAsync(request.CreateWalletRequest);
            return result.Data;
        }
    }
}
