using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using System.Linq;
using Android.Runtime;

namespace Plugin.CloudFirestore
{
    public class WriteBatchWrapper : IWriteBatch, IEquatable<WriteBatchWrapper>
    {
        private readonly WriteBatch _writeBatch;

        public WriteBatchWrapper(WriteBatch writeBatch)
        {
            _writeBatch = writeBatch ?? throw new ArgumentNullException(nameof(writeBatch));
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
            _writeBatch.Set(document.ToNative(), documentData.ToNativeFieldValues());
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData)
        {
            _writeBatch.Set(document.ToNative(), documentData.ToNativeFieldValues());
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, params string[] mergeFields)
        {
            _writeBatch.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields));
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData, params string[] mergeFields)
        {
            _writeBatch.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields));
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, params FieldPath[] mergeFields)
        {
            _writeBatch.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeFieldPaths(new JavaList<Firebase.Firestore.FieldPath>(mergeFields.Select(x => x.ToNative()))));
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData, params FieldPath[] mergeFields)
        {
            _writeBatch.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeFieldPaths(new JavaList<Firebase.Firestore.FieldPath>(mergeFields.Select(x => x.ToNative()))));
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, bool merge)
        {
            if (merge)
            {
                SetData(document, documentData);
                return;
            }

            _writeBatch.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.Merge());
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData, bool merge)
        {
            if (merge)
            {
                return Set(document, documentData);
            }

            _writeBatch.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.Merge());
            return this;
        }

        public void UpdateData(IDocumentReference document, object fields)
        {
            _writeBatch.Update(document.ToNative(), fields.ToNativeFieldValues());
        }

        public IWriteBatch Update<T>(IDocumentReference document, T fields)
        {
            _writeBatch.Update(document.ToNative(), fields.ToNativeFieldValues());
            return this;
        }

        public void UpdateData(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public IWriteBatch Update(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues)
        {
            _writeBatch.Update(document.ToNative(), field, value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return this;
        }

        public void UpdateData(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public IWriteBatch Update(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            _writeBatch.Update(document.ToNative(), field?.ToNative(), value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return this;
        }

        public void DeleteDocument(IDocumentReference document)
        {
            Delete(document);
        }

        public IWriteBatch Delete(IDocumentReference document)
        {
            _writeBatch.Delete(document.ToNative());
            return this;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as WriteBatchWrapper);
        }

        public bool Equals(WriteBatchWrapper? other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_writeBatch, other._writeBatch)) return true;
            return _writeBatch.Equals(other._writeBatch);
        }

        public override int GetHashCode()
        {
            return _writeBatch?.GetHashCode() ?? 0;
        }
    }
}
