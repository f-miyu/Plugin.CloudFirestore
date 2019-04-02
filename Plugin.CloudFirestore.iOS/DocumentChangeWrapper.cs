using System;
using Firebase.CloudFirestore;
namespace Plugin.CloudFirestore
{
    public class DocumentChangeWrapper : IDocumentChange
    {
        public IDocumentSnapshot Document => _documentChange.Document == null ? null : new DocumentSnapshotWrapper(_documentChange.Document);

        public int NewIndex => (int)_documentChange.NewIndex;

        public int OldIndex => (int)_documentChange.OldIndex;

        public DocumentChangeType Type
        {
            get
            {
                switch (_documentChange.Type)
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

        private readonly DocumentChange _documentChange;

        public DocumentChangeWrapper(DocumentChange documentChange)
        {
            _documentChange = documentChange;
        }
    }
}
