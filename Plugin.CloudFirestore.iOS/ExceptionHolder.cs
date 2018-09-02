using System;
using Foundation;
namespace Plugin.CloudFirestore
{
    public class ExceptionHolder : NSError
    {
        public Exception Exception { get; }

        public ExceptionHolder(Exception exception)
        {
            Exception = exception;
        }
    }
}
