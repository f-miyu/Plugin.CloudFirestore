using System;
using System.Threading.Tasks;
using Firebase.CloudFirestore;

namespace Plugin.CloudFirestore
{
    public class WriteBatchWrapper : IWriteBatch
    {
        private readonly WriteBatch _writeBatch;

        public WriteBatchWrapper(WriteBatch writeBatch)
        {
            _writeBatch = writeBatch;
        }

        public void Commit(CompletionHandler handler)
        {
            _writeBatch.Commit((error) =>
            {
                handler?.Invoke(error == null ? null : ExceptionMapper.Map(error));
            });
        }

        public Task CommitAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            _writeBatch.Commit((error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
                }
                else
                {
                    tcs.SetResult(true);
                }
            });

            return tcs.Task;
        }

        public void SetData<T>(IDocumentReference document, T documentData) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper);
        }

        public void SetData<T>(IDocumentReference document, T documentData, string[] mergeFields) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper, mergeFields);
        }

        public void SetData<T>(IDocumentReference document, T documentData, bool merge) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper, merge);
        }

        public void UpdateData<T>(IDocumentReference document, T fields) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.UpdateData(fields.ToNativeFieldValues(), (DocumentReference)wrapper);
        }

        public void UpdateData<T>(IDocumentReference document, string field, T value, params object[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.UpdateData(fields, (DocumentReference)wrapper);
        }

        public void DeleteDocument(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.DeleteDocument((DocumentReference)wrapper);
        }
    }
}
