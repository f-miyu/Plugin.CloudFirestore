using System;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public static class CloudFirestore
    {
        private static Firebase.FirebaseApp _app;

        internal static FirebaseFirestore Instance => FirebaseFirestore.GetInstance(_app);

        public static void Init()
        {
            if (_app == null)
            {
                var context = Android.App.Application.Context;

                var baseOptions = Firebase.FirebaseOptions.FromResource(context);
                var options = new Firebase.FirebaseOptions.Builder(baseOptions).SetProjectId(baseOptions.StorageBucket.Split('.')[0]).Build();

                _app = Firebase.FirebaseApp.InitializeApp(context, options, "Plugin.CloudFirestore");
            }
        }
    }
}
