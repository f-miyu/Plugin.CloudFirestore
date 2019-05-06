using System;
namespace Plugin.CloudFirestore
{
    internal class JavaException : Java.Lang.Exception
    {
        public Exception Exception { get; }

        public JavaException(Exception exception)
        {
            Exception = exception;
        }
    }
}
