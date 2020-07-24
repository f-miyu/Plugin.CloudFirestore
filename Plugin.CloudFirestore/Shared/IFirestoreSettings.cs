using System;
namespace Plugin.CloudFirestore
{
    public interface IFirestoreSettings
    {
        [Obsolete("This setting now defaults to true and will be removed in a future release.")]
        bool AreTimestampsInSnapshotsEnabled { get; }
        string Host { get; }
        bool IsPersistenceEnabled { get; }
        bool IsSslEnabled { get; }
        long CacheSizeBytes { get; }
    }
}
