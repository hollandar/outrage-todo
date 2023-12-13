using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadence.UI.Services
{
    public enum ToastType
    {
        Default,
        Success,
        Danger,
        Warning
    }

    public class ToasterService
    {
        private readonly IJSRuntime jsRuntime;

        public ToasterService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task Publish(string message, ToastType type = ToastType.Default, int duration = 5000)
        {
            await jsRuntime.InvokeVoidAsync("Toaster.publish", message, type switch { 
                ToastType.Success => "success",
                ToastType.Danger => "danger",
                ToastType.Warning => "warning",
                _ => "default"
            }, duration);
        }
    }
}
