using System;
using Firebase.CloudFirestore;

namespace Plugin.CloudFirestore
{
    public class SnapshotMetadataWrapper : ISnapshotMetadata
    {
        public bool HasPendingWrites => _snapshotMetadata.HasPendingWrites;

        public bool IsFromCache => _snapshotMetadata.IsFromCache;

        private readonly SnapshotMetadata _snapshotMetadata;

        public SnapshotMetadataWrapper(SnapshotMetadata snapshotMetadata)
        {
            _snapshotMetadata = snapshotMetadata;
        }
    }
}
