using System;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using Foundation;

namespace Plugin.CloudFirestore
{
    public class CloudFirestoreImplementation : ICloudFirestore
    {
        public CloudFirestoreImplementation()
        {
            var settings = Firestore.SharedInstance.Settings;
            settings.TimestampsInSnapshotsEnabled = true;
            Firestore.SharedInstance.Settings = settings;
        }

        public IFirestore Instance => new FirestoreWrapper(Firestore.SharedInstance);

        public IFieldValue FieldValue { get; } = new FieldValueWrapper();

        public IFirestore GetInstance(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return new FirestoreWrapper(Firestore.Create(app));
        }
    }
}