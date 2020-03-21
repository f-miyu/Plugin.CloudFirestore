using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using Foundation;

namespace Plugin.CloudFirestore
{
    public class CloudFirestoreImplementation : ICloudFirestore
    {
        public IFirestore Instance => FirestoreProvider.Firestore;

        public IFirestore GetInstance(string appName)
        {
            return FirestoreProvider.GetFirestore(appName);
        }
    }
}