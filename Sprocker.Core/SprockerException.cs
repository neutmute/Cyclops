using System;

namespace Sprocker.Core
{
    public class SprockerException : Exception
    {
        public SprockerException(string message, Exception innerException) :base (message,innerException)
        {
            
        }
    }
}
