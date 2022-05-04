using System;
namespace Plugin.CloudFirestore
{
    public interface IFirestoreSettings
    {
        string Host { get; }
        bool IsPersistenceEnabled { get; }
        bool IsSslEnabled { get; }
        long CacheSizeBytes { get; }
    }
}
