using Common.Logging;
using System;
namespace Cyclops.ExtensionMethods
{
    public static class ILogExtensions
    {
        public static bool IsEnabled(this ILog target, LogLevel level)
        {
            bool result;
            switch (level)
            {
                case LogLevel.Trace:
                    result = target.IsTraceEnabled;
                    break;
                case LogLevel.Debug:
                    result = target.IsDebugEnabled;
                    break;
                case LogLevel.Info:
                    result = target.IsInfoEnabled;
                    break;
                case LogLevel.Warn:
                    result = target.IsWarnEnabled;
                    break;
                case LogLevel.Error:
                    result = target.IsErrorEnabled;
                    break;
                case LogLevel.Fatal:
                    result = target.IsFatalEnabled;
                    break;
                default:
                    throw new NotSupportedException();
            }
            return result;
        }
        public static void Log(this ILog target, LogLevel level, Action<FormatMessageHandler> action)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    target.Trace(action);
                    break;
                case LogLevel.Debug:
                    target.Debug(action);
                    break;
                case LogLevel.Info:
                    target.Info(action);
                    break;
                case LogLevel.Warn:
                    target.Warn(action);
                    break;
                case LogLevel.Error:
                    target.Error(action);
                    break;
                case LogLevel.Fatal:
                    target.Fatal(action);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}