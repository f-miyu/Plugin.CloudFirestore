using System;
using System.Threading.Tasks;
using Firebase.CloudFirestore;

namespace Plugin.CloudFirestore
{
    public class WriteBatchWrapper : IWriteBatch
    {
        public WriteBatch WriteBatch { get; }

        public WriteBatchWrapper(WriteBatch writeBatch)
        {
            WriteBatch = writeBatch;
        }

        public void Commit(CompletionHandler handler)
        {
            WriteBatch.Commit((error) =>
            {
                handler?.Invoke(error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });
        }

        public Task CommitAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            WriteBatch.Commit((error) =>
            {
                if (error != null)
                {
                    tcs.SetException(new CloudFirestoreException(error.LocalizedDescription));
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
            WriteBatch.SetData(documentData.ToNativeFieldValues(), wrapper.DocumentReference);
        }

        public void SetData<T>(IDocumentReference document, T documentData, string[] mergeFields) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            WriteBatch.SetData(documentData.ToNativeFieldValues(), wrapper.DocumentReference, mergeFields);
        }

        public void SetData<T>(IDocumentReference document, T documentData, bool merge) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            WriteBatch.SetData(documentData.ToNativeFieldValues(), wrapper.DocumentReference, merge);
        }

        public void UpdateData<T>(IDocumentReference document, T fields) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            WriteBatch.UpdateData(fields.ToNativeFieldValues(), wrapper.DocumentReference);
        }

        public void UpdateData<T>(IDocumentReference document, string field, T value, params object[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);
            var wrapper = (DocumentReferenceWrapper)document;
            WriteBatch.UpdateData(fields, wrapper.DocumentReference);
        }

        public void DeleteDocument(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            WriteBatch.DeleteDocument(wrapper.DocumentReference);
        }
    }
}
