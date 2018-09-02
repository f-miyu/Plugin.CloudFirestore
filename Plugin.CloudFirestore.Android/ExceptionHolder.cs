using System;
namespace Plugin.CloudFirestore
{
    public class ExceptionHolder : Java.Lang.Exception
    {
        public Exception Exception { get; }

        public ExceptionHolder(Exception exception)
        {
            Exception = exception;
        }
    }
}
