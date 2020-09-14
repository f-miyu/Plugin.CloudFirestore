using System;
using System.Collections.Concurrent;
using Firebase.CloudFirestore;

namespace Plugin.CloudFirestore
{
    internal static class FirestoreProvider
    {
        private static ConcurrentDictionary<Firestore, Lazy<FirestoreWrapper>> _firestores = new ConcurrentDictionary<Firestore, Lazy<FirestoreWrapper>>();

        public static FirestoreWrapper Firestore { get; } = new FirestoreWrapper(Firebase.CloudFirestore.Firestore.SharedInstance);

        public static FirestoreWrapper GetFirestore(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return GetFirestore(Firebase.CloudFirestore.Firestore.Create(app));
        }

        public static FirestoreWrapper GetFirestore(Firestore firestore)
        {
            if (firestore == Firebase.CloudFirestore.Firestore.SharedInstance)
            {
                return Firestore;
            }
            return _firestores.GetOrAdd(firestore, key => new Lazy<FirestoreWrapper>(() => new FirestoreWrapper(key))).Value;
        }
    }
}
