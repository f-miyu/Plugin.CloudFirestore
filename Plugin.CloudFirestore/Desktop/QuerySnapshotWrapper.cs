﻿using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.CloudFirestore
{
    public class QuerySnapshotWrapper : IQuerySnapshot, IEquatable<QuerySnapshotWrapper>
    {
        private readonly QuerySnapshot _querySnapshot;

        public QuerySnapshotWrapper(QuerySnapshot querySnapshot)
        {
            _querySnapshot = querySnapshot ?? throw new ArgumentNullException(nameof(querySnapshot));
        }

        public int Count => (int)_querySnapshot.Count;

        public IEnumerable<IDocumentChange> DocumentChanges => _querySnapshot.Changes.Select(d => new DocumentChangeWrapper(d));

        public IEnumerable<IDocumentSnapshot> Documents => _querySnapshot.Documents.Select(d => new DocumentSnapshotWrapper(d));

        public bool IsEmpty => _querySnapshot.Count == 0;//.IsEmpty;

        public ISnapshotMetadata Metadata => throw new NotImplementedException(); //new SnapshotMetadataWrapper(_querySnapshot.Metadata);

        public IQuery Query => new QueryWrapper(_querySnapshot.Query);

        public IEnumerable<T> ToObjects<T>()
        {
            return Documents.Select(d => d.ToObject<T>()!);
        }

        public IEnumerable<T> ToObjects<T>(ServerTimestampBehavior serverTimestampBehavior)
        {
            return Documents.Select(d => d.ToObject<T>(serverTimestampBehavior)!);
        }

        public IEnumerable<IDocumentChange> GetDocumentChanges(bool includeMetadataChanges)
        {
            if (includeMetadataChanges)
                throw new NotImplementedException();
            return _querySnapshot.Changes
                .Select(d => new DocumentChangeWrapper(d));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as QuerySnapshotWrapper);
        }

        public bool Equals(QuerySnapshotWrapper? other)
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
