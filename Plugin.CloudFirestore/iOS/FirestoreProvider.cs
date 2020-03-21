using System;
using System.Collections.Concurrent;
using Firebase.CloudFirestore;

namespace Plugin.CloudFirestore
{
    internal static class FirestoreProvider
    {
        private static ConcurrentDictionary<Firestore, Lazy<FirestoreWrapper>> _firestores = new ConcurrentDictionary<Firestore, Lazy<FirestoreWrapper>>();

        public static FirestoreWrapper Firestore => _firestores.GetOrAdd(Firebase.CloudFirestore.Firestore.SharedInstance, key => new Lazy<FirestoreWrapper>(() => new FirestoreWrapper(key))).Value;

        public static FirestoreWrapper GetFirestore(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return _firestores.GetOrAdd(Firebase.CloudFirestore.Firestore.Create(app), key => new Lazy<FirestoreWrapper>(() => new FirestoreWrapper(key))).Value;
        }

        public static FirestoreWrapper GetFirestore(Firestore firestore)
        {
            return _firestores.GetOrAdd(firestore, key => new Lazy<FirestoreWrapper>(() => new FirestoreWrapper(key))).Value;
        }
    }
}
