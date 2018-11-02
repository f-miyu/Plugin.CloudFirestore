using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public class QuerySnapshotWrapper : IQuerySnapshot
    {
        public int Count => _querySnapshot.Size();

        private IEnumerable<IDocumentChange> _documentChanges;
        public IEnumerable<IDocumentChange> DocumentChanges =>
        _documentChanges ?? (_documentChanges = _querySnapshot.DocumentChanges.Select(d => new DocumentChangeWrapper(d)));

        private IEnumerable<IDocumentSnapshot> _documents;
        public IEnumerable<IDocumentSnapshot> Documents =>
        _documents ?? (_documents = _querySnapshot.Documents.Select(d => new DocumentSnapshotWrapper(d)));

        public bool IsEmpty => _querySnapshot.IsEmpty;

        private ISnapshotMetadata _metadata;
        public ISnapshotMetadata Metadata => _metadata ?? (_metadata = new SnapshotMetadataWrapper(_querySnapshot.Metadata));

        private IQuery _query;
        public IQuery Query => _query ?? (_query = new QueryWrapper(_querySnapshot.Query));

        private readonly QuerySnapshot _querySnapshot;

        public QuerySnapshotWrapper(QuerySnapshot querySnapshot)
        {
            _querySnapshot = querySnapshot;
        }

        public IEnumerable<T> ToObjects<T>() where T : class
        {
            return Documents.Select(d => d.ToObject<T>());
        }
    }
}
