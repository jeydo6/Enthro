using Enthro.Domain.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Enthro.Application.Commands
{
    public class LogoutHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IAuthenticationService _authenticationService;

        public LogoutHandler(
            IAuthenticationService authenticationService
        )
        {
            _authenticationService = authenticationService;
        }

        public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _authenticationService.LogoutAsync();

            return Unit.Value;
        }
    }
}