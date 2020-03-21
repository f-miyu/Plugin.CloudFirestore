using System;
namespace Plugin.CloudFirestore
{
    public partial class FirestoreSettings
    {
        static FirestoreSettings()
        {
            var settings = new Firebase.Firestore.FirebaseFirestoreSettings.Builder().Build();
            _deaultAreTimestampsInSnapshotsEnabled = settings.AreTimestampsInSnapshotsEnabled();
            _defaultHost = settings.Host;
            _defaultIsPersistenceEnabled = settings.IsPersistenceEnabled;
            _defaultIsSslEnabled = settings.IsSslEnabled;
        }

        internal FirestoreSettings(Firebase.Firestore.FirebaseFirestoreSettings settings)
        {
            AreTimestampsInSnapshotsEnabled = settings.AreTimestampsInSnapshotsEnabled();
            Host = settings.Host;
            IsPersistenceEnabled = settings.IsPersistenceEnabled;
            IsSslEnabled = settings.IsSslEnabled;
        }
    }
}
