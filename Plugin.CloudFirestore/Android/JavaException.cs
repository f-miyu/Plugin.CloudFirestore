using System;
namespace Plugin.CloudFirestore
{
    internal class JavaException : Java.Lang.Exception
    {
        public JavaException(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}
