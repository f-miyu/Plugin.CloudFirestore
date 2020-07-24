using System;
namespace Plugin.CloudFirestore
{
    public partial class FirestoreSettings : IFirestoreSettings, IEquatable<FirestoreSettings>
    {
        private static bool _deaultAreTimestampsInSnapshotsEnabled;
        private static string _defaultHost;
        private static bool _defaultIsPersistenceEnabled;
        private static bool _defaultIsSslEnabled;
        private static long _defaultCacheSizeBytes;

        [Obsolete("This setting now defaults to true and will be removed in a future release.")]
        public bool AreTimestampsInSnapshotsEnabled { get; set; }
        public string Host { get; set; }
        public bool IsPersistenceEnabled { get; set; }
        public bool IsSslEnabled { get; set; }
        public long CacheSizeBytes { get; set; }

        public static long CacheSizeUnlimited { get; private set; }

        public FirestoreSettings()
        {
            AreTimestampsInSnapshotsEnabled = _deaultAreTimestampsInSnapshotsEnabled;
            Host = _defaultHost;
            IsPersistenceEnabled = _defaultIsPersistenceEnabled;
            IsSslEnabled = _defaultIsSslEnabled;
            CacheSizeBytes = _defaultCacheSizeBytes;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FirestoreSettings);
        }

        public bool Equals(FirestoreSettings other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            return AreTimestampsInSnapshotsEnabled == other.AreTimestampsInSnapshotsEnabled &&
                Host == other.Host &&
                IsPersistenceEnabled == other.IsPersistenceEnabled &&
                IsSslEnabled == other.IsSslEnabled &&
                CacheSizeBytes == other.CacheSizeBytes;
        }

        public override int GetHashCode()
        {
            return AreTimestampsInSnapshotsEnabled.GetHashCode() ^
                (Host?.GetHashCode() ?? 0) ^
                IsPersistenceEnabled.GetHashCode() ^
                IsSslEnabled.GetHashCode() ^
                CacheSizeBytes.GetHashCode();
        }
    }
}