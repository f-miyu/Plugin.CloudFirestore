using System;
using System.Linq;
using Android.Runtime;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public class TransactionWrapper : ITransaction
    {
        private readonly Transaction _transaction;

        public TransactionWrapper(Transaction transaction)
        {
            _transaction = transaction;
        }

        public IDocumentSnapshot GetDocument(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            var snapshot = _transaction.Get((DocumentReference)wrapper);
            return new DocumentSnapshotWrapper(snapshot);
        }

        public void SetData(IDocumentReference document, object documentData)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues());
        }

        public void SetData(IDocumentReference document, object documentData, params string[] mergeFields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields));
        }

        public void SetData(IDocumentReference document, object documentData, params FieldPath[] mergeFields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues(), SetOptions.MergeFieldPaths(new JavaList<Firebase.Firestore.FieldPath>(mergeFields.Select(x => x.ToNative()))));
        }

        public void SetData(IDocumentReference document, object documentData, bool merge)
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
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Update((DocumentReference)wrapper, fields.ToNativeFieldValues());
        }

        public void UpdateData(IDocumentReference document, string field, object value, params object[] moreFieldsAndValues)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Update((DocumentReference)wrapper, field, value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray());
        }

        public void DeleteDocument(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.Delete((DocumentReference)wrapper);
        }
    }
}
