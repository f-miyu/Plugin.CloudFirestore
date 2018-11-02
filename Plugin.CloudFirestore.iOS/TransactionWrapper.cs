using System;
using Firebase.CloudFirestore;
using Foundation;

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
            var snapshot = _transaction.GetDocument((DocumentReference)wrapper, out var error);

            if (error != null)
            {
                throw ExceptionMapper.Map(error);
            }

            return new DocumentSnapshotWrapper(snapshot);
        }

        public void SetData<T>(IDocumentReference document, T documentData) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper);
        }

        public void SetData<T>(IDocumentReference document, T documentData, string[] mergeFields) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper, mergeFields);
        }

        public void SetData<T>(IDocumentReference document, T documentData, bool merge) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper, merge);
        }

        public void UpdateData<T>(IDocumentReference document, T fields) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.UpdateData(fields.ToNativeFieldValues(), (DocumentReference)wrapper);
        }

        public void UpdateData<T>(IDocumentReference document, string field, T value, params object[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.UpdateData(fields, (DocumentReference)wrapper);
        }

        public void DeleteDocument(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.DeleteDocument((DocumentReference)wrapper);
        }
    }
}
