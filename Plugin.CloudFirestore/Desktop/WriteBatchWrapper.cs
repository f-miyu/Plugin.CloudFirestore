using Google.Cloud.Firestore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            _writeBatch.CommitAsync().ContinueWith(t =>
            {
                handler?.Invoke(t.Exception);
            });
        }

        public Task CommitAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            _writeBatch.CommitAsync().ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    tcs.SetException(t.Exception);
                }
                else
                {
                    tcs.SetResult(true);
                }
            });
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
            _writeBatch.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields.Select(x => x.ToNative()).ToArray()));
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData, params FieldPath[] mergeFields)
        {
            _writeBatch.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields.Select(x => x.ToNative()).ToArray()));
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, bool merge)
        {
            _writeBatch.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeAll);
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData, bool merge)
        {
            _writeBatch.Set(document.ToNative(), documentData.ToNativeFieldValues(), SetOptions.MergeAll);
            return this;
        }

        public void UpdateData(IDocumentReference document, object fields)
        {
            throw new NotImplementedException();
            //_writeBatch.Update(document.ToNative(), fields, document.ToNative());
            //_writeBatch.UpdateData(fields.ToNativeFieldValues<NSObject>()!, document.ToNative());
        }

        public IWriteBatch Update<T>(IDocumentReference document, T fields)
        {
            throw new NotImplementedException();
            //_writeBatch.Update(document.ToNative(), fields);
            //_writeBatch.UpdateData(fields.ToNativeFieldValues<NSObject>()!, document.ToNative());
            //return this;
        }

        public void UpdateData(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public IWriteBatch Update(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);
            _writeBatch.Update(document.ToNative(), fields);
            return this;
        }

        public void UpdateData(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public IWriteBatch Update(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);

            _writeBatch.Update(document.ToNative(), fields);
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
