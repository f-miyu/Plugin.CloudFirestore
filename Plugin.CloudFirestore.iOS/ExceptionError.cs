using System;
using Foundation;
namespace Plugin.CloudFirestore
{
    internal class ExceptionError : NSError
    {
        public Exception Exception { get; }

        public ExceptionError(Exception exception)
        {
            Exception = exception;
        }
    }
}
