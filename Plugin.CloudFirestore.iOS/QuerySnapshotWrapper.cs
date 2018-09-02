using System;
using System.Collections.Generic;
using Firebase.CloudFirestore;
using System.Linq;

namespace Plugin.CloudFirestore
{
    public class QuerySnapshotWrapper : IQuerySnapshot
    {
        public int Count => (int)QuerySnapshot.Count;

        private IEnumerable<IDocumentChange> _documentChanges;
        public IEnumerable<IDocumentChange> DocumentChanges =>
        _documentChanges ?? (_documentChanges = QuerySnapshot.DocumentChanges.Select(d => new DocumentChangeWrapper(d)));

        private IEnumerable<IDocumentSnapshot> _documents;
        public IEnumerable<IDocumentSnapshot> Documents =>
        _documents ?? (_documents = QuerySnapshot.Documents.Select(d => new DocumentSnapshotWrapper(d)));

        public bool IsEmpty => QuerySnapshot.IsEmpty;

        private ISnapshotMetadata _metadata;
        public ISnapshotMetadata Metadata => _metadata ?? (_metadata = new SnapshotMetadataWrapper(QuerySnapshot.Metadata));

        private IQuery _query;
        public IQuery Query => _query ?? (_query = new QueryWrapper(QuerySnapshot.Query));

        public QuerySnapshot QuerySnapshot { get; }

        public QuerySnapshotWrapper(QuerySnapshot querySnapshot)
        {
            QuerySnapshot = querySnapshot;
        }

        public IEnumerable<T> ToObjects<T>() where T : class
        {
            return Documents.Select(d => d.ToObject<T>());
        }
    }
}
