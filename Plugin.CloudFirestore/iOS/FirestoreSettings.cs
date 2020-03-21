using System;
namespace Plugin.CloudFirestore
{
    public partial class FirestoreSettings
    {
        static FirestoreSettings()
        {
            var settings = new Firebase.CloudFirestore.FirestoreSettings();
            _deaultAreTimestampsInSnapshotsEnabled = settings.TimestampsInSnapshotsEnabled;
            _defaultHost = settings.Host;
            _defaultIsPersistenceEnabled = settings.PersistenceEnabled;
            _defaultIsSslEnabled = settings.SslEnabled;
        }

        internal FirestoreSettings(Firebase.CloudFirestore.FirestoreSettings settings)
        {
            AreTimestampsInSnapshotsEnabled = settings.TimestampsInSnapshotsEnabled;
            Host = settings.Host;
            IsPersistenceEnabled = settings.PersistenceEnabled;
            IsSslEnabled = settings.SslEnabled;
        }
    }
}
