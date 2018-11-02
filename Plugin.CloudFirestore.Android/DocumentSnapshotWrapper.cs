using System;
using Firebase.Firestore;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    public class DocumentSnapshotWrapper : IDocumentSnapshot
    {
        private IDictionary<string, object> _data;
        public IDictionary<string, object> Data
        {
            get
            {
                if (Exists && _data == null)
                {
                    _data = DocumentMapper.Map(_documentSnapshot);
                }
                return _data;
            }
        }

        public string Id => _documentSnapshot.Id;

        public bool Exists => _documentSnapshot.Exists();

        private ISnapshotMetadata _metadata;
        public ISnapshotMetadata Metadata => _metadata ?? (_metadata = new SnapshotMetadataWrapper(_documentSnapshot.Metadata));

        private IDocumentReference _reference;
        public IDocumentReference Reference => _reference ?? (_reference = new DocumentReferenceWrapper(_documentSnapshot.Reference));

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
