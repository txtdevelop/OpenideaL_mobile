using System;
using PSY.Innovative.Contracts;
using Xamarin.Forms;

namespace PSY.Innovative.Services
{
    public class PlatformSpecificCalls : IPlatformSpecificCalls
    {
        public void BeginInvokeOnMainThread(Action action)
        {
            Device.BeginInvokeOnMainThread(action);
        }
    }
}