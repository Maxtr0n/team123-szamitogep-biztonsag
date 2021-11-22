using System;

namespace CAFFAdapterClient.Infrastructure.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException() { }
        public InvalidPasswordException(string message) : base(message) { }
    }
}
