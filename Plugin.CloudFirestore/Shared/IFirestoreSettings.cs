using System;
namespace Plugin.CloudFirestore
{
    public interface IFirestoreSettings
    {
        bool AreTimestampsInSnapshotsEnabled { get; }
        string Host { get; }
        bool IsPersistenceEnabled { get; }
        bool IsSslEnabled { get; }
    }
}
