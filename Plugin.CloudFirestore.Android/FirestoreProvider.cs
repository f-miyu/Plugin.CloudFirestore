using System;
using System.Collections.Concurrent;
using Firebase;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    internal static class FirestoreProvider
    {
        private static ConcurrentDictionary<FirebaseFirestore, FirestoreWrapper> _firestores = new ConcurrentDictionary<FirebaseFirestore, FirestoreWrapper>();

        public static FirestoreWrapper Firestore => _firestores.GetOrAdd(FirebaseFirestore.Instance, key => new FirestoreWrapper(key));

        public static FirestoreWrapper GetFirestore(string appName)
        {
            var app = FirebaseApp.GetInstance(appName);
            return _firestores.GetOrAdd(FirebaseFirestore.GetInstance(app), key => new FirestoreWrapper(key));
        }

        public static FirestoreWrapper GetFirestore(FirebaseFirestore firestore)
        {
            return _firestores.GetOrAdd(firestore, key => new FirestoreWrapper(key));
        }
    }
}
