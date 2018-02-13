using System;

namespace PSY.Innovative.Droid.Contracts
{
    public interface IPlatformSpecificCalls
    {
        void BeginInvokeOnMainThread(Action action);
    }
}