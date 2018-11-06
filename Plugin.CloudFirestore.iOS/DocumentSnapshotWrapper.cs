using System;
using Firebase.CloudFirestore;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    public class DocumentSnapshotWrapper : IDocumentSnapshot
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

        public T ToObject<T>() where T : class
        {
            return DocumentMapper.Map<T>(_documentSnapshot);
        }
    }
}
