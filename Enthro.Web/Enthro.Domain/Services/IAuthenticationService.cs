using Enthro.Domain.Dto;
using System.Threading.Tasks;

namespace Enthro.Domain.Services
{
    public interface IAuthenticationService
    {
        Task LoginAsync(LoginDto login);

        Task LogoutAsync();
    }
}