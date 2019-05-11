using System;
using System.Collections.Concurrent;
using Firebase;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    internal static class FirestoreProvider
    {
        private static ConcurrentDictionary<FirebaseFirestore, Lazy<FirestoreWrapper>> _firestores = new ConcurrentDictionary<FirebaseFirestore, Lazy<FirestoreWrapper>>();

        public static FirestoreWrapper Firestore => _firestores.GetOrAdd(FirebaseFirestore.Instance, key => new Lazy<FirestoreWrapper>(() => new FirestoreWrapper(key))).Value;

        public static FirestoreWrapper GetFirestore(string appName)
        {
            var app = FirebaseApp.GetInstance(appName);
            return _firestores.GetOrAdd(FirebaseFirestore.GetInstance(app), key => new Lazy<FirestoreWrapper>(() => new FirestoreWrapper(key))).Value;
        }

        public static FirestoreWrapper GetFirestore(FirebaseFirestore firestore)
        {
            return _firestores.GetOrAdd(firestore, key => new Lazy<FirestoreWrapper>(() => new FirestoreWrapper(key))).Value;
        }
    }
}
