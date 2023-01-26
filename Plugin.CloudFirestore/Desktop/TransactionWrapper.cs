using Google.Cloud.Firestore;
using System;
using System.Linq;

namespace Plugin.CloudFirestore
{
    public class TransactionWrapper : ITransaction, IEquatable<TransactionWrapper>
    {
        private readonly Transaction _transaction;

        public TransactionWrapper(Transaction transaction)
        {
            _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
        }

        public IDocumentSnapshot GetDocument(IDocumentReference document)
        {
            return Get(document);
        }

        public IDocumentSnapshot Get(IDocumentReference document)
        {
            var snapshot = _transaction.GetSnapshotAsync(document.ToNative()).GetAwaiter().GetResult();
            return new DocumentSnapshotWrapper(snapshot!);
        }

        public void SetData(IDocumentReference document, object documentData)
        {
            _transaction.Set(document.ToNative(), documentData.ToNativeFieldValues());
        }

        public ITransaction Set<T>(IDocumentReference document, T documentData)
        {
            _transaction.Set(document.ToNative(), documentData.ToNativeFieldValues());
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, params string[] mergeFields)
        {
            _transaction.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields));
        }

        public ITransaction Set<T>(IDocumentReference document, T documentData, params string[] mergeFields)
        {
            _transaction.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields));
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, params FieldPath[] mergeFields)
        {
            _transaction.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields.Select(x => x.ToNative()).ToArray()));
        }

        public ITransaction Set<T>(IDocumentReference document, T documentData, params FieldPath[] mergeFields)
        {
            _transaction.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields.Select(x => x.ToNative()).ToArray()));
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, bool merge)
        {
            _transaction.Set(document.ToNative(), documentData.ToNativeFieldValues(), merge ? SetOptions.MergeAll : SetOptions.Overwrite);
        }

        public ITransaction Set<T>(IDocumentReference document, T documentData, bool merge)
        {
            _transaction.Set(document.ToNative(), documentData.ToNativeFieldValues(), merge ? SetOptions.MergeAll : SetOptions.Overwrite);
            return this;
        }

        public void UpdateData(IDocumentReference document, object fields)
        {
            _transaction.Update(document.ToNative(), fields.ToNativeFieldValues());
        }

        public ITransaction Update<T>(IDocumentReference document, T fields)
        {
            _transaction.Update(document.ToNative(), fields.ToNativeFieldValues());
            return this;
        }

        public void UpdateData(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public ITransaction Update(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);
            _transaction.Update(document.ToNative(), fields.ToNativeFieldValues());
            return this;
        }

        public void UpdateData(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public ITransaction Update(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);
            _transaction.Update(document.ToNative(), fields.ToNativeFieldValues());
            return this;
        }

        public void DeleteDocument(IDocumentReference document)
        {
            Delete(document);
        }

        public ITransaction Delete(IDocumentReference document)
        {
            _transaction.Delete(document.ToNative());
            return this;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as TransactionWrapper);
        }

        public bool Equals(TransactionWrapper? other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_transaction, other._transaction)) return true;
            return _transaction.Equals(other._transaction);
        }

        public override int GetHashCode()
        {
            return _transaction?.GetHashCode() ?? 0;
        }
    }
}
