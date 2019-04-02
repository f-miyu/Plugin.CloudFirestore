using System;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.CloudFirestore
{
    public class DocumentReferenceWrapper : IDocumentReference
    {
        public string Id => _documentReference.Id;

        public string Path => _documentReference.Path;

        public ICollectionReference Parent => _documentReference.Parent == null ? null : new CollectionReferenceWrapper(_documentReference.Parent);

        private readonly DocumentReference _documentReference;

        public DocumentReferenceWrapper(DocumentReference documentReference)
        {
            _documentReference = documentReference;
        }

        public static explicit operator DocumentReference(DocumentReferenceWrapper wrapper)
        {
            return wrapper._documentReference;
        }

        public ICollectionReference GetCollection(string collectionPath)
        {
            var collectionReference = _documentReference.GetCollection(collectionPath);
            return new CollectionReferenceWrapper(collectionReference);
        }

        public void GetDocument(DocumentSnapshotHandler handler)
        {
            _documentReference.GetDocument((snapshot, error) =>
            {
                handler?.Invoke(snapshot == null ? null : new DocumentSnapshotWrapper(snapshot),
                                error == null ? null : ExceptionMapper.Map(error));

            });
        }

        public Task<IDocumentSnapshot> GetDocumentAsync()
        {
            var tcs = new TaskCompletionSource<IDocumentSnapshot>();

            _documentReference.GetDocument((snapshot, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
                }
                else
                {
                    tcs.SetResult(snapshot == null ? null : new DocumentSnapshotWrapper(snapshot));
                }
            });

            return tcs.Task;
        }

        public void SetData<T>(T documentData, CompletionHandler handler) where T : class
        {
            _documentReference.SetData(documentData.ToNativeFieldValues(), (error) =>
            {
                handler?.Invoke(error == null ? null : ExceptionMapper.Map(error));
            });
        }

        public Task SetDataAsync<T>(T documentData) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.SetData(documentData.ToNativeFieldValues(), (error) =>
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

        public void SetData<T>(T documentData, IEnumerable<string> mergeFields, CompletionHandler handler) where T : class
        {
            _documentReference.SetData(documentData.ToNativeFieldValues(), mergeFields.ToArray(), (error) =>
            {
                handler?.Invoke(error == null ? null : ExceptionMapper.Map(error));
            });
        }

        public Task SetDataAsync<T>(T documentData, IEnumerable<string> mergeFields) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.SetData(documentData.ToNativeFieldValues(), mergeFields.ToArray(), (error) =>
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

        public void SetData<T>(T documentData, bool merge, CompletionHandler handler) where T : class
        {
            _documentReference.SetData(documentData.ToNativeFieldValues(), merge, (error) =>
            {
                handler?.Invoke(error == null ? null : ExceptionMapper.Map(error));
            });
        }

        public Task SetDataAsync<T>(T documentData, bool merge) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.SetData(documentData.ToNativeFieldValues(), merge, (error) =>
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

        public void UpdateData<T>(T fields, CompletionHandler handler) where T : class
        {
            _documentReference.UpdateData(fields.ToNativeFieldValues(), (error) =>
            {
                handler?.Invoke(error == null ? null : ExceptionMapper.Map(error));
            });
        }

        public Task UpdateDataAsync<T>(T fields) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.UpdateData(fields.ToNativeFieldValues(), (error) =>
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

        public void UpdateData<T>(string field, T value, CompletionHandler handler, params object[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);

            _documentReference.UpdateData(fields, (error) =>
            {
                handler?.Invoke(error == null ? null : ExceptionMapper.Map(error));
            });
        }

        public Task UpdateDataAsync<T>(string field, T value, params object[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);

            var tcs = new TaskCompletionSource<bool>();

            _documentReference.UpdateData(fields, (error) =>
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

        public void DeleteDocument(CompletionHandler handler)
        {
            _documentReference.DeleteDocument((error) =>
            {
                handler?.Invoke(error == null ? null : ExceptionMapper.Map(error));
            });
        }

        public Task DeleteDocumentAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            _documentReference.DeleteDocument((error) =>
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

        public IListenerRegistration AddSnapshotListener(DocumentSnapshotHandler listener)
        {
            var registration = _documentReference.AddSnapshotListener((snapshot, error) =>
            {
                listener?.Invoke(snapshot == null ? null : new DocumentSnapshotWrapper(snapshot),
                                 error == null ? null : ExceptionMapper.Map(error));
            });

            return new ListenerRegistrationWrapper(registration);
        }

        public IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, DocumentSnapshotHandler listener)
        {
            var registration = _documentReference.AddSnapshotListener(includeMetadataChanges, (snapshot, error) =>
            {
                listener?.Invoke(snapshot == null ? null : new DocumentSnapshotWrapper(snapshot),
                                 error == null ? null : ExceptionMapper.Map(error));
            });

            return new ListenerRegistrationWrapper(registration);
        }
    }
}
