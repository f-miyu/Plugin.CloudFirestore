using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public class QuerySnapshotWrapper : IQuerySnapshot, IEquatable<QuerySnapshotWrapper>
    {
        public int Count => _querySnapshot.Size();

        public IEnumerable<IDocumentChange> DocumentChanges => _querySnapshot.DocumentChanges?.Select(d => new DocumentChangeWrapper(d));

        public IEnumerable<IDocumentSnapshot> Documents => _querySnapshot.Documents?.Select(d => new DocumentSnapshotWrapper(d));

        public bool IsEmpty => _querySnapshot.IsEmpty;

        public ISnapshotMetadata Metadata => _querySnapshot.Metadata == null ? null : new SnapshotMetadataWrapper(_querySnapshot.Metadata);

        public IQuery Query => _querySnapshot.Query == null ? null : new QueryWrapper(_querySnapshot.Query);

        private readonly QuerySnapshot _querySnapshot;

        public QuerySnapshotWrapper(QuerySnapshot querySnapshot)
        {
            _querySnapshot = querySnapshot;
        }

        public IEnumerable<T> ToObjects<T>()
        {
            return Documents.Select(d => d.ToObject<T>());
        }

        public IEnumerable<T> ToObjects<T>(ServerTimestampBehavior serverTimestampBehavior)
        {
            return Documents.Select(d => d.ToObject<T>(serverTimestampBehavior));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as QuerySnapshotWrapper);
        }

        public bool Equals(QuerySnapshotWrapper other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_querySnapshot, other._querySnapshot)) return true;
            return _querySnapshot.Equals(other._querySnapshot);
        }

        public override int GetHashCode()
        {
            return _querySnapshot?.GetHashCode() ?? 0;
        }
    }
}
