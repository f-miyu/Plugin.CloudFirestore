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
                FirebaseFirestore firestore;
                if (string.IsNullOrEmpty(CloudFirestore.DefaultAppName))
                {
                    firestore = FirebaseFirestore.Instance;
                }
                else
                {
                    var app = FirebaseApp.GetInstance(CloudFirestore.DefaultAppName);
                    firestore = FirebaseFirestore.GetInstance(app);
                }
                return new FirestoreWrapper(firestore);
            }
        }

        public IFirestore GetInstance(string appName)
        {
            var app = FirebaseApp.GetInstance(appName);
            return new FirestoreWrapper(FirebaseFirestore.GetInstance(app));
        }
    }
}
