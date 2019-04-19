using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using System.Linq;
using Android.Runtime;

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
            _writeBatch.Commit().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public Task CommitAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            _writeBatch.Commit().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                if (task.IsSuccessful)
                {
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            return tcs.Task;
        }

        public void SetData(IDocumentReference document, object documentData)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues());
        }

        public void SetData(IDocumentReference document, object documentData, params string[] mergeFields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields));
        }

        public void SetData(IDocumentReference document, object documentData, params FieldPath[] mergeFields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues(), SetOptions.MergeFieldPaths(new JavaList<Firebase.Firestore.FieldPath>(mergeFields.Select(x => x.ToNative()))));
        }

        public void SetData(IDocumentReference document, object documentData, bool merge)
        {
            if (merge)
            {
                SetData(document, documentData);
                return;
            }

            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.Set((DocumentReference)wrapper, documentData.ToNativeFieldValues(), SetOptions.Merge());
        }

        public void UpdateData(IDocumentReference document, object fields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.Update((DocumentReference)wrapper, fields.ToNativeFieldValues());
        }

        public void UpdateData(IDocumentReference document, string field, object value, params object[] moreFieldsAndValues)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.Update((DocumentReference)wrapper, field, value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray());
        }

        public void UpdateData(IDocumentReference document, FieldPath field, object value, params object[] moreFieldsAndValues)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.Update((DocumentReference)wrapper, field.ToNative(), value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray());
        }

        public void DeleteDocument(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.Delete((DocumentReference)wrapper);
        }
    }
}
