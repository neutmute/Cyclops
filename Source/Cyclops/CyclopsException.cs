using System;

namespace Cyclops
{
    public class CyclopsException : Exception
    {
        public CyclopsException(string message, Exception innerException) :base (message,innerException)
        {
            
        }

        /// <summary>
        /// Easily a new CyclopsException without needing a string.Format
        /// </summary>
        public static CyclopsException Create(Exception innerException, string format, params object[] args)
        {
            string message = string.Format(format, args);
            CyclopsException exception = new CyclopsException(message, innerException);
            return exception;
        }

        public static CyclopsException Create(string format, params object[] args)
        {
            return Create(null, format, args);
        }
    }
}
