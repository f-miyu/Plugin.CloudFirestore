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
            _transaction = transaction;
        }

        public IDocumentSnapshot GetDocument(IDocumentReference document)
        {
            return Get(document);
        }

        public IDocumentSnapshot Get(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            var snapshot = _transaction.Get((DocumentReference)wrapper);
            return new DocumentSnapshotWrapper(snapshot);
        }

        public void SetData(IDocumentReference document, object documentData)
        {
            Set(document, documentData);
        }

        public void Set(IDocumentReference document, object documentData)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues());
        }

        public void SetData(IDocumentReference document, object documentData, params string[] mergeFields)
        {
            Set(document, documentData, mergeFields);
        }

        public void Set(IDocumentReference document, object documentData, params string[] mergeFields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields));
        }

        public void SetData(IDocumentReference document, object documentData, params FieldPath[] mergeFields)
        {
            Set(document, documentData, mergeFields);
        }

        public void Set(IDocumentReference document, object documentData, params FieldPath[] mergeFields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues(), SetOptions.MergeFieldPaths(new JavaList<Firebase.Firestore.FieldPath>(mergeFields.Select(x => x.ToNative()))));
        }

        public void SetData(IDocumentReference document, object documentData, bool merge)
        {
            Set(document, documentData, merge);
        }

        public void Set(IDocumentReference document, object documentData, bool merge)
        {
            if (merge)
            {
                SetData(document, documentData);
                return;
            }

            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues(), SetOptions.Merge());
        }

        public void UpdateData(IDocumentReference document, object fields)
        {
            Update(document, fields);
        }

        public void Update(IDocumentReference document, object fields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Update((DocumentReference)wrapper, fields.ToNativeFieldValues());
        }

        public void UpdateData(IDocumentReference document, string field, object value, params object[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public void Update(IDocumentReference document, string field, object value, params object[] moreFieldsAndValues)
        {
            if (moreFieldsAndValues == null)
            {
                throw new ArgumentNullException(nameof(moreFieldsAndValues));
            }

            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Update((DocumentReference)wrapper, field, value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray());
        }

        public void UpdateData(IDocumentReference document, FieldPath field, object value, params object[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public void Update(IDocumentReference document, FieldPath field, object value, params object[] moreFieldsAndValues)
        {
            if (moreFieldsAndValues == null)
            {
                throw new ArgumentNullException(nameof(moreFieldsAndValues));
            }
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Update((DocumentReference)wrapper, field?.ToNative(), value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray());
        }

        public void DeleteDocument(IDocumentReference document)
        {
            Delete(document);
        }

        public void Delete(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Delete((DocumentReference)wrapper);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TransactionWrapper);
        }

        public bool Equals(TransactionWrapper other)
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
