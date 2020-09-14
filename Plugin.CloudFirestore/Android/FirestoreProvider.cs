using System;
using System.Collections.Concurrent;
using Firebase;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    internal static class FirestoreProvider
    {
        private static ConcurrentDictionary<FirebaseFirestore, Lazy<FirestoreWrapper>> _firestores = new ConcurrentDictionary<FirebaseFirestore, Lazy<FirestoreWrapper>>();

        public static FirestoreWrapper Firestore { get; } = new FirestoreWrapper(FirebaseFirestore.Instance);

        public static FirestoreWrapper GetFirestore(string appName)
        {
            var app = FirebaseApp.GetInstance(appName);
            return GetFirestore(FirebaseFirestore.GetInstance(app));
        }

        public static FirestoreWrapper GetFirestore(FirebaseFirestore firestore)
        {
            if (firestore == FirebaseFirestore.Instance)
            {
                return Firestore;
            }
            return _firestores.GetOrAdd(firestore, key => new Lazy<FirestoreWrapper>(() => new FirestoreWrapper(key))).Value;
        }
    }
}
