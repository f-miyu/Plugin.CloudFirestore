using System;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public class DocumentChangeWrapper : IDocumentChange
    {
        private IDocumentSnapshot _document;
        public IDocumentSnapshot Document => _document ?? (_document = new DocumentSnapshotWrapper(DocumentChange.Document));

        public int NewIndex => DocumentChange.NewIndex;

        public int OldIndex => DocumentChange.OldIndex;

        public DocumentChangeType Type
        {
            get
            {
                var type = DocumentChange.GetType();
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

        public DocumentChange DocumentChange { get; }

        public DocumentChangeWrapper(DocumentChange documentChange)
        {
            DocumentChange = documentChange;
        }
    }
}
