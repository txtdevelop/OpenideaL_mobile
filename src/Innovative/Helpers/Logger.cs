using System;
using MvvmCross.Platform;

namespace PSY.Innovative.Helpers
{
    public static class Logger
    {
        private const string AppTag = "PSY_INNOVATIVE";
        public static void TaggedTrace(string tag, string message, params object[] args)
        {
            Mvx.TaggedTrace(tag, message, args);
        }

        public static void TaggedWarning(string tag, string message, params object[] args)
        {
            Mvx.TaggedWarning(tag, message, args);
        }

        public static void TaggedError(string tag, string message, params object[] args)
        {
            Mvx.TaggedError(tag, message, args);
        }

        public static void TaggedException(string tag, string message, Exception exception, params object[] args)
        {
            TaggedError(tag, $"Message: {message}, Exception : {exception.GetBaseException()}", args);
        }

        public static void Trace(string message, params object[] args)
        {
            TaggedTrace(AppTag, message, args);
        }

        public static void Warning(string message, params object[] args)
        {
            TaggedWarning(AppTag, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            TaggedError(AppTag, message, args);
        }

        public static void Exception(string message, Exception exception, params object[] args)
        {
            TaggedException(AppTag, message, exception, args);
        }
    }
}