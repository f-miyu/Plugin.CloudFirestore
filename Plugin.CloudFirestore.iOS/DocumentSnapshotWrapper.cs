using System;
using Firebase.CloudFirestore;
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
                    _data = DocumentMapper.Map(DocumentSnapshot);
                }
                return _data;
            }
        }

        public string Id => DocumentSnapshot.Id;

        public bool Exists => DocumentSnapshot.Exists;

        private ISnapshotMetadata _metadata;
        public ISnapshotMetadata Metadata => _metadata ?? (_metadata = new SnapshotMetadataWrapper(DocumentSnapshot.Metadata));

        private IDocumentReference _reference;
        public IDocumentReference Reference => _reference ?? (_reference = new DocumentReferenceWrapper(DocumentSnapshot.Reference));

        public DocumentSnapshot DocumentSnapshot { get; }

        public DocumentSnapshotWrapper(DocumentSnapshot documentSnapshot)
        {
            DocumentSnapshot = documentSnapshot;
        }

        public T ToObject<T>() where T : class
        {
            return DocumentMapper.Map<T>(DocumentSnapshot);
        }
    }
}
