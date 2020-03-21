using System;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public class DocumentChangeWrapper : IDocumentChange, IEquatable<DocumentChangeWrapper>
    {
        public IDocumentSnapshot Document => _documentChange.Document == null ? null : new DocumentSnapshotWrapper(_documentChange.Document);

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

        public override bool Equals(object obj)
        {
            return Equals(obj as DocumentChangeWrapper);
        }

        public bool Equals(DocumentChangeWrapper other)
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
