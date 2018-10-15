using System;
using Firebase.CloudFirestore;
using Foundation;

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
            var snapshot = Transaction.GetDocument(wrapper.DocumentReference, out var error);

            if (error != null)
            {
                throw new CloudFirestoreException(error.LocalizedDescription);
            }

            return new DocumentSnapshotWrapper(snapshot);
        }

        public void SetData<T>(IDocumentReference document, T documentData) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            Transaction.SetData(documentData.ToNativeFieldValues(), wrapper.DocumentReference);
        }

        public void SetData<T>(IDocumentReference document, T documentData, string[] mergeFields) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            Transaction.SetData(documentData.ToNativeFieldValues(), wrapper.DocumentReference, mergeFields);
        }

        public void SetData<T>(IDocumentReference document, T documentData, bool merge) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            Transaction.SetData(documentData.ToNativeFieldValues(), wrapper.DocumentReference, merge);
        }

        public void UpdateData<T>(IDocumentReference document, T fields) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            Transaction.UpdateData(fields.ToNativeFieldValues(), wrapper.DocumentReference);
        }

        public void UpdateData<T>(IDocumentReference document, string field, T value, params object[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);
            var wrapper = (DocumentReferenceWrapper)document;
            Transaction.UpdateData(fields, wrapper.DocumentReference);
        }

        public void DeleteDocument(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            Transaction.DeleteDocument(wrapper.DocumentReference);
        }
    }
}
