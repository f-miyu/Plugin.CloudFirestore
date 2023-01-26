using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using System;
using System.Collections.Concurrent;

namespace Plugin.CloudFirestore
{
    internal static class FirestoreProvider
    {
        private static Google.Cloud.Firestore.FirestoreDb SharedInstance;
        private static ConcurrentDictionary<Google.Cloud.Firestore.FirestoreDb, Lazy<FirestoreWrapper>> _firestores =
            new ConcurrentDictionary<Google.Cloud.Firestore.FirestoreDb, Lazy<FirestoreWrapper>>();

        internal static Google.Apis.Auth.OAuth2.ICredential Credentials { get; set; }
        internal static string AppName { get; set; }

        public static FirestoreWrapper Firestore { get; internal set; }

        public static Google.Cloud.Firestore.FirestoreDb GetFirestore()
        {
            if (FirestoreProvider.Credentials is null || string.IsNullOrEmpty(AppName))
            {
                throw new InvalidOperationException("Credentials is null. Call Plugin.CloudFirestore.Setup.Init() before using Firestore.");
            }
            var builder = new FirestoreClientBuilder
            {
                Credential = FirestoreProvider.Credentials, // await GetCredentials(),
            };
            return SharedInstance ??= FirestoreDb.Create(AppName, builder.Build());
        }

        public static FirestoreWrapper GetFirestore(string appName)
        {
            var builder = new FirestoreClientBuilder
            {
                Credential = FirestoreProvider.Credentials, // await GetCredentials(),
            };
            return GetFirestore(FirestoreDb.Create(appName, builder.Build()));
        }
        public static FirestoreWrapper GetFirestore(Google.Cloud.Firestore.FirestoreDb firestore)
        {
            if (firestore == SharedInstance)
            {
                return Firestore;
            }
            return _firestores.GetOrAdd(firestore, key =>
                new Lazy<FirestoreWrapper>(() => new FirestoreWrapper(key))).Value;
        }
    }
}
