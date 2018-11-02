using System;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using Foundation;

namespace Plugin.CloudFirestore
{
    public class CloudFirestoreImplementation : ICloudFirestore
    {
        public IFirestore Instance
        {
            get
            {
                Firestore firestore;
                if (string.IsNullOrEmpty(CloudFirestore.DefaultAppName))
                {
                    firestore = Firestore.SharedInstance;
                }
                else
                {
                    var app = Firebase.Core.App.From(CloudFirestore.DefaultAppName);
                    firestore = Firestore.Create(app);
                }
                return new FirestoreWrapper(firestore);
            }
        }

        public IFirestore GetInstance(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return new FirestoreWrapper(Firestore.Create(app));
        }
    }
}