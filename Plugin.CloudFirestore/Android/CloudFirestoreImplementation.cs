using Firebase.Firestore;

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
