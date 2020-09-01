using System;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public class SnapshotMetadataWrapper : ISnapshotMetadata, IEquatable<SnapshotMetadataWrapper>
    {
        private readonly SnapshotMetadata _snapshotMetadata;

        public SnapshotMetadataWrapper(SnapshotMetadata snapshotMetadata)
        {
            _snapshotMetadata = snapshotMetadata ?? throw new ArgumentNullException(nameof(snapshotMetadata));
        }

        public bool HasPendingWrites => _snapshotMetadata.HasPendingWrites;

        public bool IsFromCache => _snapshotMetadata.IsFromCache;

        public override bool Equals(object? obj)
        {
            return Equals(obj as SnapshotMetadataWrapper);
        }

        public bool Equals(SnapshotMetadataWrapper? other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_snapshotMetadata, other._snapshotMetadata)) return true;
            return _snapshotMetadata.Equals(other._snapshotMetadata);
        }

        public override int GetHashCode()
        {
            return _snapshotMetadata?.GetHashCode() ?? 0;
        }
    }
}
