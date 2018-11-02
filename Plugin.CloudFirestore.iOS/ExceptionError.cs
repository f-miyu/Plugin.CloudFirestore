using System;
using Foundation;
namespace Plugin.CloudFirestore
{
    public class ExceptionError : NSError
    {
        public Exception Exception { get; }

        public ExceptionError(Exception exception)
        {
            Exception = exception;
        }
    }
}
