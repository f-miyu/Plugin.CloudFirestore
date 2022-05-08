using System;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using System.Linq;
using Foundation;

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

        public void SetData(IDocumentReference document, object documentData)
        {
            _writeBatch.SetData(documentData.ToNativeFieldValues<NSString>()!, document.ToNative());
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData)
        {
            _writeBatch.SetData(documentData.ToNativeFieldValues<NSString>()!, document.ToNative());
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, params string[] mergeFields)
        {
            _writeBatch.SetData(documentData.ToNativeFieldValues<NSString>()!, document.ToNative(), mergeFields);
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData, params string[] mergeFields)
        {
            _writeBatch.SetData(documentData.ToNativeFieldValues<NSString>()!, document.ToNative(), mergeFields);
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, params FieldPath[] mergeFields)
        {
            _writeBatch.SetData(documentData.ToNativeFieldValues<NSString>()!, document.ToNative(), mergeFields.Select(x => x.ToNative()).ToArray());
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData, params FieldPath[] mergeFields)
        {
            _writeBatch.SetData(documentData.ToNativeFieldValues<NSString>()!, document.ToNative(), mergeFields.Select(x => x.ToNative()).ToArray());
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, bool merge)
        {
            _writeBatch.SetData(documentData.ToNativeFieldValues<NSString>()!, document.ToNative(), merge);
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData, bool merge)
        {
            _writeBatch.SetData(documentData.ToNativeFieldValues<NSString>()!, document.ToNative(), merge);
            return this;
        }

        public void UpdateData(IDocumentReference document, object fields)
        {
            _writeBatch.UpdateData(fields.ToNativeFieldValues<NSObject>()!, document.ToNative());
        }

        public IWriteBatch Update<T>(IDocumentReference document, T fields)
        {
            _writeBatch.UpdateData(fields.ToNativeFieldValues<NSObject>()!, document.ToNative());
            return this;
        }

        public void UpdateData(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public IWriteBatch Update(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);
            _writeBatch.UpdateData(fields, document.ToNative());
            return this;
        }

        public void UpdateData(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public IWriteBatch Update(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);
            _writeBatch.UpdateData(fields, document.ToNative());
            return this;
        }

        public void DeleteDocument(IDocumentReference document)
        {
            Delete(document);
        }

        public IWriteBatch Delete(IDocumentReference document)
        {
            _writeBatch.DeleteDocument(document.ToNative());
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
