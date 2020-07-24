using System;
using Firebase.Firestore;
using Firebase;
using System.Collections.Concurrent;

namespace Plugin.CloudFirestore
{
    public class CloudFirestoreImplementation : ICloudFirestore
    {
        public IFirestore Instance => FirestoreProvider.Firestore;

        public IFirestore GetInstance(string appName)
        {
            return FirestoreProvider.GetFirestore(appName);
        }

        public void SetLoggingEnabled(bool loggingEnabled)
        {
            FirebaseFirestore.SetLoggingEnabled(loggingEnabled);
        }
    }
}
