using System;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public class DocumentChangeWrapper : IDocumentChange
    {
        public IDocumentSnapshot Document => new DocumentSnapshotWrapper(_documentChange.Document);

        public int NewIndex => _documentChange.NewIndex;

        public int OldIndex => _documentChange.OldIndex;

        public DocumentChangeType Type
        {
            get
            {
                var type = _documentChange.GetType();
                if (type == DocumentChange.Type.Added)
                {
                    return DocumentChangeType.Added;
                }
                if (type == DocumentChange.Type.Modified)
                {
                    return DocumentChangeType.Modified;
                }
                if (type == DocumentChange.Type.Removed)
                {
                    return DocumentChangeType.Removed;
                }

                throw new InvalidOperationException();
            }
        }

        private readonly DocumentChange _documentChange;

        public DocumentChangeWrapper(DocumentChange documentChange)
        {
            _documentChange = documentChange;
        }
    }
}
