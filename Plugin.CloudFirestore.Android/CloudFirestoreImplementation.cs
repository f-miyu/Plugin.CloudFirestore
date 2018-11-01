using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using Android.Runtime;

namespace Plugin.CloudFirestore
{
    public class CloudFirestoreImplementation : ICloudFirestore
    {
        public IInstance Instance => new InstanceWrapper();

        public IInstance GetInstance(string appName)
        {
            return new InstanceWrapper(appName);
        }
    }
}
