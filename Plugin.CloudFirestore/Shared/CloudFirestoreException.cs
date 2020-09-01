using System;
namespace Plugin.CloudFirestore
{
    public class CloudFirestoreException : Exception
    {
        public CloudFirestoreException(string message, ErrorType errorType) : base(message)
        {
            ErrorType = errorType;
        }

        public CloudFirestoreException(string message, ErrorType errorType, Exception innerException) : base(message, innerException)
        {
            ErrorType = errorType;
        }

        public ErrorType ErrorType { get; }
    }
}
