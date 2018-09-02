using System;
namespace Plugin.CloudFirestore
{
    public class CloudFirestoreException : Exception
    {
        public CloudFirestoreException()
        {
        }

        public CloudFirestoreException(string message) : base(message)
        {
        }
    }
}
