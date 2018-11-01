using System;
using Firebase.Firestore;
using Android.Content;

namespace Plugin.CloudFirestore
{
    public static class CloudFirestore
    {
        public static string DefaultAppName { get; set; }

        private static bool _hasInitialized;

        public static void Init(Context context, string appName)
        {
            if (!_hasInitialized)
            {
                _hasInitialized = true;

                var baseOptions = Firebase.FirebaseOptions.FromResource(context);
                var options = new Firebase.FirebaseOptions.Builder(baseOptions).SetProjectId(baseOptions.StorageBucket.Split('.')[0]).Build();

                Firebase.FirebaseApp.InitializeApp(context, options, appName);

                DefaultAppName = appName;
            }
        }

    }
}
