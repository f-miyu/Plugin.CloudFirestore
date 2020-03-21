using System;
using Firebase.CloudFirestore;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    public class DocumentSnapshotWrapper : IDocumentSnapshot, IEquatable<DocumentSnapshotWrapper>
    {
        public IDictionary<string, object> Data => Exists ? DocumentMapper.Map(_documentSnapshot) : null;

        public string Id => _documentSnapshot.Id;

        public bool Exists => _documentSnapshot.Exists;

        public ISnapshotMetadata Metadata => new SnapshotMetadataWrapper(_documentSnapshot.Metadata);

        public IDocumentReference Reference => new DocumentReferenceWrapper(_documentSnapshot.Reference);

        private readonly DocumentSnapshot _documentSnapshot;

        public DocumentSnapshotWrapper(DocumentSnapshot documentSnapshot)
        {
            _documentSnapshot = documentSnapshot;
        }

        public static explicit operator DocumentSnapshot(DocumentSnapshotWrapper wrapper)
        {
            return wrapper._documentSnapshot;
        }

        public IDictionary<string, object> GetData(ServerTimestampBehavior serverTimestampBehavior)
        {
            return Exists ? DocumentMapper.Map(_documentSnapshot, serverTimestampBehavior) : null;
        }

        public T ToObject<T>()
        {
            return DocumentMapper.Map<T>(_documentSnapshot);
        }

        public T ToObject<T>(ServerTimestampBehavior serverTimestampBehavior)
        {
            return DocumentMapper.Map<T>(_documentSnapshot, serverTimestampBehavior);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as DocumentSnapshotWrapper);
        }

        public bool Equals(DocumentSnapshotWrapper other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_documentSnapshot, other._documentSnapshot)) return true;
            return _documentSnapshot.Equals(other._documentSnapshot);
        }

        public override int GetHashCode()
        {
            return _documentSnapshot?.GetHashCode() ?? 0;
        }
    }
}
