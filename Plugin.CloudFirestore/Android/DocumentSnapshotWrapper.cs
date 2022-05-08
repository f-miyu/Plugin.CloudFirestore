using System;
using Firebase.Firestore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Plugin.CloudFirestore
{
    public class DocumentSnapshotWrapper : IDocumentSnapshot, IEquatable<DocumentSnapshotWrapper>
    {
        private readonly DocumentSnapshot _documentSnapshot;

        public DocumentSnapshotWrapper(DocumentSnapshot documentSnapshot)
        {
            _documentSnapshot = documentSnapshot ?? throw new ArgumentNullException(nameof(documentSnapshot));
        }

        public IDictionary<string, object?>? Data => Exists ? DocumentMapper.Map(_documentSnapshot) : null;

        public string Id => _documentSnapshot.Id;

        public bool Exists => _documentSnapshot.Exists();

        public ISnapshotMetadata Metadata => new SnapshotMetadataWrapper(_documentSnapshot.Metadata);

        public IDocumentReference Reference => new DocumentReferenceWrapper(_documentSnapshot.Reference);

        public IDictionary<string, object?>? GetData(ServerTimestampBehavior serverTimestampBehavior)
        {
            return Exists ? DocumentMapper.Map(_documentSnapshot, serverTimestampBehavior) : null;
        }

        [return: MaybeNull]
        public T ToObject<T>()
        {
            return DocumentMapper.Map<T>(_documentSnapshot);
        }

        [return: MaybeNull]
        public T ToObject<T>(ServerTimestampBehavior serverTimestampBehavior)
        {
            return DocumentMapper.Map<T>(_documentSnapshot, serverTimestampBehavior);
        }

        [return: MaybeNull]
        public T Get<T>(string field)
        {
            var value = _documentSnapshot.Get(field).ToFieldValue(new DocumentFieldInfo<T>());
            return value is not null ? (T)value : default;
        }

        [return: MaybeNull]
        public T Get<T>(string field, ServerTimestampBehavior serverTimestampBehavior)
        {
            var value = _documentSnapshot.Get(field, serverTimestampBehavior.ToNative()).ToFieldValue(new DocumentFieldInfo<T>());
            return value is not null ? (T)value : default;
        }

        [return: MaybeNull]
        public T Get<T>(FieldPath field)
        {
            var value = _documentSnapshot.Get(field?.ToNative()).ToFieldValue(new DocumentFieldInfo<T>());
            return value is not null ? (T)value : default;
        }

        [return: MaybeNull]
        public T Get<T>(FieldPath field, ServerTimestampBehavior serverTimestampBehavior)
        {
            var value = _documentSnapshot.Get(field?.ToNative(), serverTimestampBehavior.ToNative()).ToFieldValue(new DocumentFieldInfo<T>());
            return value is not null ? (T)value : default;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DocumentSnapshotWrapper);
        }

        public bool Equals(DocumentSnapshotWrapper? other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_documentSnapshot, other._documentSnapshot)) return true;
            return _documentSnapshot.Equals(other._documentSnapshot);
        }

        public override int GetHashCode()
        {
            return _documentSnapshot?.GetHashCode() ?? 0;
        }

        DocumentSnapshot IDocumentSnapshot.ToNative()
        {
            return _documentSnapshot;
        }
    }
}
