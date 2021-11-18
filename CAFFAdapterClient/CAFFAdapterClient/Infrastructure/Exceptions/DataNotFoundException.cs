using System;

namespace CAFFAdapterClient.Infrastructure.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string message) : base(message) { }
    }
}
