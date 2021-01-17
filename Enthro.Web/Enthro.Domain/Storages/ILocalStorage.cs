using System;
using System.Threading.Tasks;

namespace Enthro.Domain.Storages
{
    public interface ILocalStorage
    {
        Task<String> GetItemAsync(String key);

        Task SetItemAsync(String key, String value);
    }
}
