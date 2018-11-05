using System;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using Foundation;

namespace Plugin.CloudFirestore
{
    public class CloudFirestoreImplementation : ICloudFirestore
    {
        public IFirestore Instance => new FirestoreWrapper(Firestore.SharedInstance);

        public IFirestore GetInstance(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return new FirestoreWrapper(Firestore.Create(app));
        }
    }
}