using Enthro.Application.Models;
using MediatR;

namespace Enthro.Application.Commands
{
    public class LoginCommand : IRequest
    {
        public LoginCommand(LoginModel model)
        {
            Model = model;
        }

        public LoginModel Model { get; }
    }
}
