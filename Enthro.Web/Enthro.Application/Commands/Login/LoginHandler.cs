using AutoMapper;
using Enthro.Domain.Dto;
using Enthro.Domain.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Enthro.Application.Commands
{
    public class LoginHandler : IRequestHandler<LoginCommand>
    {
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authenticationService;

        public LoginHandler(
            IMapper mapper,
            IAuthenticationService authenticationService
        )
        {
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<Unit> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginDto = _mapper.Map<LoginDto>(request.Model);

            await _authenticationService.LoginAsync(loginDto);

            return Unit.Value;
        }
    }
}