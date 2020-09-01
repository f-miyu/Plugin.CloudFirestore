using System;
using Firebase.CloudFirestore;
namespace Plugin.CloudFirestore
{
    public class DocumentChangeWrapper : IDocumentChange, IEquatable<DocumentChangeWrapper>
    {
        private readonly DocumentChange _documentChange;

        public DocumentChangeWrapper(DocumentChange documentChange)
        {
            _documentChange = documentChange ?? throw new ArgumentNullException(nameof(documentChange));
        }

        public IDocumentSnapshot Document => new DocumentSnapshotWrapper(_documentChange.Document);

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

        public override bool Equals(object? obj)
        {
            return Equals(obj as DocumentChangeWrapper);
        }

        public bool Equals(DocumentChangeWrapper? other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_documentChange, other._documentChange)) return true;
            return _documentChange.Equals(other._documentChange);
        }

        public override int GetHashCode()
        {
            return _documentChange?.GetHashCode() ?? 0;
        }
    }
}
