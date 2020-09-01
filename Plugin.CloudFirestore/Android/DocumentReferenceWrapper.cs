using System;
using System.Threading.Tasks;
using Android.Runtime;
using Firebase.Firestore;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.CloudFirestore
{
    public class DocumentReferenceWrapper : IDocumentReference, IEquatable<DocumentReferenceWrapper>
    {
        private readonly DocumentReference _documentReference;

        public DocumentReferenceWrapper(DocumentReference documentReference)
        {
            _documentReference = documentReference ?? throw new ArgumentNullException(nameof(documentReference));
        }

        public string Id => _documentReference.Id;

        public string Path => _documentReference.Path;

        public ICollectionReference Parent => new CollectionReferenceWrapper(_documentReference.Parent);

        public IFirestore Firestore => FirestoreProvider.GetFirestore(_documentReference.Firestore);

        public ICollectionReference GetCollection(string collectionPath)
        {
            return Collection(collectionPath);
        }

        public ICollectionReference Collection(string collectionPath)
        {
            var collectionReference = _documentReference.Collection(collectionPath);
            return new CollectionReferenceWrapper(collectionReference);
        }

        public void GetDocument(DocumentSnapshotHandler handler)
        {
            var tcs = new TaskCompletionSource<IDocumentSnapshot>();

            _documentReference.Get().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                var snapshot = !task.IsSuccessful ? null : task.Result.JavaCast<DocumentSnapshot>();
                handler?.Invoke(snapshot == null ? null : new DocumentSnapshotWrapper(snapshot),
                                task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public void GetDocument(Source source, DocumentSnapshotHandler handler)
        {
            var tcs = new TaskCompletionSource<IDocumentSnapshot>();

            _documentReference.Get(source.ToNative()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                var snapshot = !task.IsSuccessful ? null : task.Result.JavaCast<DocumentSnapshot>();
                handler?.Invoke(snapshot == null ? null : new DocumentSnapshotWrapper(snapshot),
                                task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public Task<IDocumentSnapshot> GetDocumentAsync()
        {
            return GetAsync();
        }

        public Task<IDocumentSnapshot> GetAsync()
        {
            var tcs = new TaskCompletionSource<IDocumentSnapshot>();

            _documentReference.Get().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                if (task.IsSuccessful)
                {
                    var snapshot = task.Result.JavaCast<DocumentSnapshot>();
                    tcs.SetResult(new DocumentSnapshotWrapper(snapshot!));
                }
                else
                {
                    tcs.SetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            return tcs.Task;
        }

        public Task<IDocumentSnapshot> GetDocumentAsync(Source source)
        {
            return GetAsync(source);
        }

        public Task<IDocumentSnapshot> GetAsync(Source source)
        {
            var tcs = new TaskCompletionSource<IDocumentSnapshot>();

            _documentReference.Get(source.ToNative()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                if (task.IsSuccessful)
                {
                    var snapshot = task.Result.JavaCast<DocumentSnapshot>();
                    tcs.SetResult(new DocumentSnapshotWrapper(snapshot!));
                }
                else
                {
                    tcs.SetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            return tcs.Task;
        }

        public void SetData(object documentData, CompletionHandler handler)
        {
            _documentReference.Set(documentData.ToNativeFieldValues()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public Task SetDataAsync(object documentData)
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Set(documentData.ToNativeFieldValues()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public Task SetAsync<T>(T documentData)
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Set(documentData.ToNativeFieldValues()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public void SetData(object documentData, CompletionHandler handler, params string[] mergeFields)
        {
            _documentReference.Set(documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields)).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public void SetData(object documentData, CompletionHandler handler, params FieldPath[] mergeFields)
        {
            _documentReference.Set(documentData.ToNativeFieldValues(), SetOptions.MergeFieldPaths(new JavaList<Firebase.Firestore.FieldPath>(mergeFields.Select(x => x.ToNative())))).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public Task SetDataAsync(object documentData, params string[] mergeFields)
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Set(documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields)).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public Task SetAsync<T>(T documentData, params string[] mergeFields)
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Set(documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields)).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public Task SetDataAsync(object documentData, params FieldPath[] mergeFields)
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Set(documentData.ToNativeFieldValues(), SetOptions.MergeFieldPaths(new JavaList<Firebase.Firestore.FieldPath>(mergeFields.Select(x => x.ToNative())))).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public Task SetAsync<T>(T documentData, params FieldPath[] mergeFields)
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Set(documentData.ToNativeFieldValues(), SetOptions.MergeFieldPaths(new JavaList<Firebase.Firestore.FieldPath>(mergeFields.Select(x => x.ToNative())))).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public void SetData(object documentData, bool merge, CompletionHandler handler)
        {
            if (!merge)
            {
                SetData(documentData, handler);
                return;
            }

            _documentReference.Set(documentData.ToNativeFieldValues(), SetOptions.Merge()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public Task SetDataAsync(object documentData, bool merge)
        {
            if (!merge)
            {
                return SetDataAsync(documentData);
            }

            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Set(documentData.ToNativeFieldValues(), SetOptions.Merge()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public Task SetAsync<T>(T documentData, bool merge)
        {
            if (!merge)
            {
                return SetAsync(documentData);
            }

            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Set(documentData.ToNativeFieldValues(), SetOptions.Merge()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public void UpdateData(object fields, CompletionHandler handler)
        {
            _documentReference.Update(fields.ToNativeFieldValues()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public Task UpdateDataAsync(object fields)
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Update(fields.ToNativeFieldValues()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public Task UpdateAsync<T>(T fields)
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Update(fields.ToNativeFieldValues()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public void UpdateData(string field, object? value, CompletionHandler handler, params object?[] moreFieldsAndValues)
        {
            _documentReference.Update(field, value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public void UpdateData(FieldPath field, object? value, CompletionHandler handler, params object?[] moreFieldsAndValues)
        {
            _documentReference.Update(field?.ToNative(), value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public Task UpdateDataAsync(string field, object? value, params object?[] moreFieldsAndValues)
        {
            return UpdateAsync(field, value, moreFieldsAndValues);
        }

        public Task UpdateAsync(string field, object? value, params object?[] moreFieldsAndValues)
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Update(field, value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public Task UpdateDataAsync(FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            return UpdateAsync(field, value, moreFieldsAndValues);
        }

        public Task UpdateAsync(FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Update(field?.ToNative(), value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public void DeleteDocument(CompletionHandler handler)
        {
            _documentReference.Delete().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public Task DeleteDocumentAsync()
        {
            return DeleteAsync();
        }

        public Task DeleteAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.Delete().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public IListenerRegistration AddSnapshotListener(DocumentSnapshotHandler listener)
        {
            var registration = _documentReference.AddSnapshotListener(new EventHandlerListener<DocumentSnapshot>((value, error) =>
            {
                listener?.Invoke(value == null ? null : new DocumentSnapshotWrapper(value),
                                 error == null ? null : ExceptionMapper.Map(error));
            }));

            return new ListenerRegistrationWrapper(registration);
        }

        public IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, DocumentSnapshotHandler listener)
        {
            var registration = _documentReference.AddSnapshotListener(includeMetadataChanges ? MetadataChanges.Include : MetadataChanges.Exclude, new EventHandlerListener<DocumentSnapshot>((value, error) =>
           {
               listener?.Invoke(value == null ? null : new DocumentSnapshotWrapper(value),
                                error == null ? null : ExceptionMapper.Map(error));
           }));

            return new ListenerRegistrationWrapper(registration);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DocumentReferenceWrapper);
        }

        public bool Equals(DocumentReferenceWrapper? other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_documentReference, other._documentReference)) return true;
            return _documentReference.Equals(other._documentReference);
        }

        public override int GetHashCode()
        {
            return _documentReference?.GetHashCode() ?? 0;
        }

        DocumentReference IDocumentReference.ToNative()
        {
            return _documentReference;
        }
    }
}
