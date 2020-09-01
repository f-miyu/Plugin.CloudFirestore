using System;
using System.Linq;
using Android.Runtime;
using Firebase.Firestore;

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
            var snapshot = _transaction.Get(document.ToNative());
            return new DocumentSnapshotWrapper(snapshot);
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
            _transaction.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeFieldPaths(new JavaList<Firebase.Firestore.FieldPath>(mergeFields.Select(x => x.ToNative()))));
        }

        public ITransaction Set<T>(IDocumentReference document, T documentData, params FieldPath[] mergeFields)
        {
            _transaction.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeFieldPaths(new JavaList<Firebase.Firestore.FieldPath>(mergeFields.Select(x => x.ToNative()))));
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, bool merge)
        {
            if (merge)
            {
                SetData(document, documentData);
                return;
            }

            _transaction.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.Merge());
        }

        public ITransaction Set<T>(IDocumentReference document, T documentData, bool merge)
        {
            if (merge)
            {
                return Set(document, documentData);
            }

            _transaction.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.Merge());
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
            _transaction.Update(document.ToNative(), field, value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return this;
        }

        public void UpdateData(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public ITransaction Update(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            _transaction.Update(document.ToNative(), field?.ToNative(), value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray());
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
