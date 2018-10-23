using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using System.Linq;

namespace Plugin.CloudFirestore
{
    public class WriteBatchWrapper : IWriteBatch
    {
        private WriteBatch WriteBatch { get; }

        public WriteBatchWrapper(WriteBatch writeBatch)
        {
            WriteBatch = writeBatch;
        }

        public void Commit(CompletionHandler handler)
        {
            WriteBatch.Commit().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : new CloudFirestoreException(task.Exception.Message));
            }));
        }

        public Task CommitAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            WriteBatch.Commit().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                if (task.IsSuccessful)
                {
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetException(new CloudFirestoreException(task.Exception.Message));
                }
            }));

            return tcs.Task;
        }

        public void SetData<T>(IDocumentReference document, T documentData) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            WriteBatch.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues());
        }

        public void SetData<T>(IDocumentReference document, T documentData, string[] mergeFields) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            WriteBatch.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields));
        }

        public void SetData<T>(IDocumentReference document, T documentData, bool merge) where T : class
        {
            if (merge)
            {
                SetData(document, documentData);
                return;
            }

            var wrapper = (DocumentReferenceWrapper)document;
            WriteBatch.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues(), SetOptions.Merge());
        }

        public void UpdateData<T>(IDocumentReference document, T fields) where T : class
        {
            var wrapper = (DocumentReferenceWrapper)document;
            WriteBatch.Update((DocumentReference)wrapper, fields.ToNativeFieldValues());
        }

        public void UpdateData<T>(IDocumentReference document, string field, T value, params object[] moreFieldsAndValues)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            WriteBatch.Update((DocumentReference)wrapper, field, value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray());
        }

        public void DeleteDocument(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            WriteBatch.Delete((DocumentReference)wrapper);
        }
    }
}
