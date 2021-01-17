using Enthro.Domain.Storages;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Enthro.Infrastructure.Storages
{
    public class LocalStorage : ILocalStorage
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorage(
            IJSRuntime jSRuntime
        )
        {
            _jsRuntime = jSRuntime;
        }

        public async Task<String> GetItemAsync(String key)
        {
            return await _jsRuntime.InvokeAsync<String>("localStorage.getItem", key);
        }

        public async Task SetItemAsync(String key, String value)
        {
            if (value == null)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
            }
            else
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
            }
        }
    }
}
