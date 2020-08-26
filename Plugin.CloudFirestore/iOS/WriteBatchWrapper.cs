using System;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using System.Linq;

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
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper);
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper);
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, params string[] mergeFields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper, mergeFields);
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData, params string[] mergeFields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper, mergeFields);
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, params FieldPath[] mergeFields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper, mergeFields.Select(x => x.ToNative()).ToArray());
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData, params FieldPath[] mergeFields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper, mergeFields.Select(x => x.ToNative()).ToArray());
            return this;
        }

        public void SetData(IDocumentReference document, object documentData, bool merge)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper, merge);
        }

        public IWriteBatch Set<T>(IDocumentReference document, T documentData, bool merge)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.SetData(documentData.ToNativeFieldValues(), (DocumentReference)wrapper, merge);
            return this;
        }

        public void UpdateData(IDocumentReference document, object fields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.UpdateData(fields.ToNativeFieldValues(), (DocumentReference)wrapper);
        }

        public IWriteBatch Update<T>(IDocumentReference document, T fields)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.UpdateData(fields.ToNativeFieldValues(), (DocumentReference)wrapper);
            return this;
        }

        public void UpdateData(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public IWriteBatch Update(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.UpdateData(fields, (DocumentReference)wrapper);
            return this;
        }

        public void UpdateData(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            Update(document, field, value, moreFieldsAndValues);
        }

        public IWriteBatch Update(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.UpdateData(fields, (DocumentReference)wrapper);
            return this;
        }

        public void DeleteDocument(IDocumentReference document)
        {
            Delete(document);
        }

        public IWriteBatch Delete(IDocumentReference document)
        {
            var wrapper = (DocumentReferenceWrapper)document;
            _writeBatch.DeleteDocument((DocumentReference)wrapper);
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
