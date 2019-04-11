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

        public void SetData(IDocumentReference document, object documentData)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper);
        }

        public void SetData(IDocumentReference document, object documentData, string[] mergeFields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper, mergeFields);
        }

        public void SetData(IDocumentReference document, object documentData, bool merge)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper, merge);
        }

        public void UpdateData(IDocumentReference document, object fields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _transaction.UpdateData(fields.ToNativeFieldValues(), (DocumentReference)wrapper);
        }

        public void UpdateData(IDocumentReference document, string field, object value, params object[] moreFieldsAndValues)
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
