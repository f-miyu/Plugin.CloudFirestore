using System;
using System.Collections.Concurrent;
using Firebase.CloudFirestore;

namespace Plugin.CloudFirestore
{
    internal static class FirestoreProvider
    {
        private static ConcurrentDictionary<Firestore, FirestoreWrapper> _firestores = new ConcurrentDictionary<Firestore, FirestoreWrapper>();

        public static FirestoreWrapper Firestore => _firestores.GetOrAdd(Firebase.CloudFirestore.Firestore.SharedInstance, key => new FirestoreWrapper(key));

        public static FirestoreWrapper GetFirestore(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return _firestores.GetOrAdd(Firebase.CloudFirestore.Firestore.Create(app), key => new FirestoreWrapper(key));
        }

        public static FirestoreWrapper GetFirestore(Firestore firestore)
        {
            return _firestores.GetOrAdd(firestore, key => new FirestoreWrapper(key));
        }
    }
}
