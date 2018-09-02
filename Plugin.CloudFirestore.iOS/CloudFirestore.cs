using System;
namespace Plugin.CloudFirestore
{
    public static class CloudFirestore
    {
        public static Firebase.CloudFirestore.Firestore Instance => Firebase.CloudFirestore.Firestore.SharedInstance;

        public static void Init()
        {
            Firebase.Core.App.Configure();
        }
    }
}
