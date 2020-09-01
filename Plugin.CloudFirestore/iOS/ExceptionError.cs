using System;
using Foundation;
namespace Plugin.CloudFirestore
{
    internal class ExceptionError : NSError
    {
        public ExceptionError(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}
