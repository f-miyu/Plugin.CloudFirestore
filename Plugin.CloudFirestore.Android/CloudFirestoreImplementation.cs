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
                var app = FirebaseApp.GetInstance(CloudFirestore.DefaultAppName);
                return new FirestoreWrapper(FirebaseFirestore.GetInstance(app));
            }
        }

        public IFirestore GetInstance(string appName)
        {
            var app = FirebaseApp.GetInstance(appName);
            return new FirestoreWrapper(FirebaseFirestore.GetInstance(app));
        }
    }
}
