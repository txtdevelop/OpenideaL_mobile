using System;

namespace PSY.Innovative.Contracts
{
    public interface IPlatformSpecificCalls
    {
        void BeginInvokeOnMainThread(Action action);
    }
}