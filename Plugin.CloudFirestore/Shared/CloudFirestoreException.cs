using System;
namespace Plugin.CloudFirestore
{
    public class CloudFirestoreException : Exception
    {
        public ErrorType ErrorType { get; }

        public CloudFirestoreException(string message, ErrorType errorType) : base(message)
        {
            ErrorType = errorType;
        }

        public CloudFirestoreException(string message, ErrorType errorType, Exception innerException) : base(message, innerException)
        {
            ErrorType = errorType;
        }
    }
}
