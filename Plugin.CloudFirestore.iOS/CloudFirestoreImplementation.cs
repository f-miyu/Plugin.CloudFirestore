using System;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using Foundation;

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