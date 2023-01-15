using System;

namespace Plugin.CloudFirestore
{
    public class CloudFirestoreImplementation : ICloudFirestore
    {
        public IFirestore Instance => FirestoreProvider.Firestore;

        public CloudFirestoreImplementation()
        {

        }
        public IFirestore GetInstance(string appName)
        {
            return FirestoreProvider.GetFirestore(appName);
        }

        public void SetLoggingEnabled(bool loggingEnabled)
        {
            throw new NotImplementedException();
        }
    }
}