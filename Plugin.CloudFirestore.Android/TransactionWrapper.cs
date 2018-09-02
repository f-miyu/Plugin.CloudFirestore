using System;
using System.Linq;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public class TransactionWrapper : ITransaction
    {
        public Transaction Transaction { get; }

        public TransactionWrapper(Transaction transaction)
        {
            Transaction = transaction;
        }

        public IDocumentSnapshot GetDocument(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            var snapshot = Transaction.Get(wrapper.DocumentReference);
            return new DocumentSnapshotWrapper(snapshot);
        }

        public void SetData<T>(IDocumentReference document, T documentData) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            Transaction.Set(wrapper.DocumentReference, documentData.ToNativeFieldValues());
        }

        public void SetData<T>(IDocumentReference document, T documentData, string[] mergeFields) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            Transaction.Set(wrapper.DocumentReference, documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields));
        }

        public void SetData<T>(IDocumentReference document, T documentData, bool merge) where T : class
        {
            if (merge)
            {
                SetData(document, documentData);
                return;
            }

            var wrapper = (DocumentReferenceWrapper)document;
            Transaction.Set(wrapper.DocumentReference, documentData.ToNativeFieldValues(), SetOptions.Merge());
        }

        public void UpdateData<T>(IDocumentReference document, T fields) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            Transaction.Update(wrapper.DocumentReference, fields.ToNativeFieldValues());
        }

        public void UpdateData<T>(IDocumentReference document, string field, T value, params object[] moreFieldsAndValues)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            Transaction.Update(wrapper.DocumentReference, field, value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray());
        }

        public void DeleteDocument(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            Transaction.Delete(wrapper.DocumentReference);
        }
    }
}
