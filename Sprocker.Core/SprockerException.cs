using System;

namespace Sprocker.Core
{
    public class SprockerException : Exception
    {
        public SprockerException(string message, Exception innerException) :base (message,innerException)
        {
            
        }

        /// <summary>
        /// Easily a new SprockerException without needing a string.Format
        /// </summary>
        public static SprockerException Create(Exception innerException, string format, params object[] args)
        {
            string message = string.Format(format, args);
            SprockerException exception = new SprockerException(message, innerException);
            return exception;
        }

        public static SprockerException Create(string format, params object[] args)
        {
            return Create(null, format, args);
        }
    }
}
