using System;
namespace Plugin.CloudFirestore
{
    public interface ISnapshotMetadata
    {
        bool HasPendingWrites { get; }
        bool IsFromCache { get; }
    }
}
