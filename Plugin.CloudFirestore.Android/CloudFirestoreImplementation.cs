using System;
using Firebase.Firestore;
using Firebase;

namespace Plugin.CloudFirestore
{
    public class CloudFirestoreImplementation : ICloudFirestore
    {
        public IFirestore Instance
        {
            get
            {
                return new FirestoreWrapper(FirebaseFirestore.Instance);
            }
        }

        public IFirestore GetInstance(string appName)
        {
            var app = FirebaseApp.GetInstance(appName);
            return new FirestoreWrapper(FirebaseFirestore.GetInstance(app));
        }
    }
}
