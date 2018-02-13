using System;
using System.Diagnostics;
using MvvmCross.Platform.Platform;

#if __ANDROID__
using Android.Util;
#endif

namespace PSY.Innovative.Droid
{
    public class DebugTrace : IMvxTrace
    {
        public void Trace(MvxTraceLevel level, string tag, Func<string> message)
        {
            Trace(level, tag, message());
        }

        public void Trace(MvxTraceLevel level, string tag, string message)
        {
#if __ANDROID__
            Log.Info(tag, message);
#elif __UNIFIED__
            Console.WriteLine(tag + ":" + level + ":" + message);
#endif
            Debug.WriteLine(tag + ":" + level + ":" + message);
        }

        public void Trace(MvxTraceLevel level, string tag, string message, params object[] args)
        {
            try
            {
#if __ANDROID__
                Log.Info(tag, message, args);
#elif __UNIFIED__
                Console.WriteLine(tag + ":" + level + ":" + message, args);
#endif
                Debug.WriteLine(tag + ":" + level + ":" + message, args);
            }
            catch (FormatException)
            {
                Trace(MvxTraceLevel.Error, tag, "Exception during trace");
                Trace(level, tag, message);
            }
        }
    }
}