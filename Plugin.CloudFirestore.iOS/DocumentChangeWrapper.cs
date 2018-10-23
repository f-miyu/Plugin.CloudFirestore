using System;
using Firebase.CloudFirestore;
namespace Plugin.CloudFirestore
{
    public class DocumentChangeWrapper : IDocumentChange
    {
        private IDocumentSnapshot _document;
        public IDocumentSnapshot Document => _document ?? (_document = new DocumentSnapshotWrapper(DocumentChange.Document));

        public int NewIndex => (int)DocumentChange.NewIndex;

        public int OldIndex => (int)DocumentChange.OldIndex;

        public DocumentChangeType Type
        {
            get
            {
                switch (DocumentChange.Type)
                {
                    case Firebase.CloudFirestore.DocumentChangeType.Added:
                        return DocumentChangeType.Added;
                    case Firebase.CloudFirestore.DocumentChangeType.Modified:
                        return DocumentChangeType.Modified;
                    case Firebase.CloudFirestore.DocumentChangeType.Removed:
                        return DocumentChangeType.Removed;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        private DocumentChange DocumentChange { get; }

        public DocumentChangeWrapper(DocumentChange documentChange)
        {
            DocumentChange = documentChange;
        }
    }
}
