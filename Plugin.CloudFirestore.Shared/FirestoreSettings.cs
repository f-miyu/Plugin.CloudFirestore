using System;
namespace Plugin.CloudFirestore
{
    public partial class FirestoreSettings
    {
        private static bool _deaultAreTimestampsInSnapshotsEnabled;
        private static string _defaultHost;
        private static bool _defaultIsPersistenceEnabled;
        private static bool _defaultIsSslEnabled;

        public bool AreTimestampsInSnapshotsEnabled { get; set; } = _deaultAreTimestampsInSnapshotsEnabled;
        public string Host { get; set; } = _defaultHost;
        public bool IsPersistenceEnabled { get; set; } = _defaultIsPersistenceEnabled;
        public bool IsSslEnabled { get; set; } = _defaultIsSslEnabled;

        public FirestoreSettings()
        {
        }
    }
}