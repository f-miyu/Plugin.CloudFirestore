using System;

namespace Plugin.CloudFirestore
{
    public class DocumentChangeWrapper : IDocumentChange, IEquatable<DocumentChangeWrapper>
    {
        private readonly Google.Cloud.Firestore.DocumentChange _documentChange;

        public DocumentChangeWrapper(Google.Cloud.Firestore.DocumentChange documentChange)
        {
            _documentChange = documentChange ?? throw new ArgumentNullException(nameof(documentChange));
        }

        public IDocumentSnapshot Document => throw new System.NotImplementedException();//new DocumentSnapshotWrapper(_documentChange.Document );

        public int NewIndex => (int)_documentChange.NewIndex;

        public int OldIndex => (int)_documentChange.OldIndex;

        public DocumentChangeType Type
        {
            get
            {
                switch (_documentChange.ChangeType)
                {
                    case Google.Cloud.Firestore.DocumentChange.Type.Added:
                        return DocumentChangeType.Added;
                    case Google.Cloud.Firestore.DocumentChange.Type.Modified:
                        return DocumentChangeType.Modified;
                    case Google.Cloud.Firestore.DocumentChange.Type.Removed:
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
