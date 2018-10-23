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
        private DocumentReference DocumentReference { get; }

        public DocumentReferenceWrapper(DocumentReference documentReference)
        {
            DocumentReference = documentReference;
        }

        public static explicit operator DocumentReference(DocumentReferenceWrapper wrapper)
        {
            return wrapper.DocumentReference;
        }

        public ICollectionReference GetCollection(string collectionPath)
        {
            var collectionReference = DocumentReference.GetCollection(collectionPath);
            return new CollectionReferenceWrapper(collectionReference);
        }

        public void GetDocument(DocumentSnapshotHandler handler)
        {
            DocumentReference.GetDocument((snapshot, error) =>
            {
                handler?.Invoke(snapshot == null ? null : new DocumentSnapshotWrapper(snapshot),
                                error == null ? null : new CloudFirestoreException(error.LocalizedDescription));

            });
        }

        public Task<IDocumentSnapshot> GetDocumentAsync()
        {
            var tcs = new TaskCompletionSource<IDocumentSnapshot>();

            DocumentReference.GetDocument((snapshot, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(new CloudFirestoreException(error.LocalizedDescription));
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
            DocumentReference.SetData(documentData.ToNativeFieldValues(), (error) =>
            {
                handler?.Invoke(error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });
        }

        public Task SetDataAsync<T>(T documentData) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            DocumentReference.SetData(documentData.ToNativeFieldValues(), (error) =>
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

        public void SetData<T>(T documentData, IEnumerable<string> mergeFields, CompletionHandler handler) where T : class
        {
            DocumentReference.SetData(documentData.ToNativeFieldValues(), mergeFields.ToArray(), (error) =>
            {
                handler?.Invoke(error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });
        }

        public Task SetDataAsync<T>(T documentData, IEnumerable<string> mergeFields) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            DocumentReference.SetData(documentData.ToNativeFieldValues(), mergeFields.ToArray(), (error) =>
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

        public void SetData<T>(T documentData, bool merge, CompletionHandler handler) where T : class
        {
            DocumentReference.SetData(documentData.ToNativeFieldValues(), merge, (error) =>
            {
                handler?.Invoke(error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });
        }

        public Task SetDataAsync<T>(T documentData, bool merge) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            DocumentReference.SetData(documentData.ToNativeFieldValues(), merge, (error) =>
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

        public void UpdateData<T>(T fields, CompletionHandler handler) where T : class
        {
            DocumentReference.UpdateData(fields.ToNativeFieldValues(), (error) =>
            {
                handler?.Invoke(error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });
        }

        public Task UpdateDataAsync<T>(T fields) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            DocumentReference.UpdateData(fields.ToNativeFieldValues(), (error) =>
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

        public void UpdateData<T>(string field, T value, CompletionHandler handler, params object[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);

            DocumentReference.UpdateData(fields, (error) =>
            {
                handler?.Invoke(error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });
        }

        public Task UpdateDataAsync<T>(string field, T value, params object[] moreFieldsAndValues)
        {
            var fields = Field.CreateFields(field, value, moreFieldsAndValues);

            var tcs = new TaskCompletionSource<bool>();

            DocumentReference.UpdateData(fields, (error) =>
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

        public void DeleteDocument(CompletionHandler handler)
        {
            DocumentReference.DeleteDocument((error) =>
            {
                handler?.Invoke(error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });
        }

        public Task DeleteDocumentAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            DocumentReference.DeleteDocument((error) =>
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

        public IListenerRegistration AddSnapshotListener(DocumentSnapshotHandler listener)
        {
            var registration = DocumentReference.AddSnapshotListener((snapshot, error) =>
            {
                listener?.Invoke(snapshot == null ? null : new DocumentSnapshotWrapper(snapshot),
                                 error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });

            return new ListenerRegistrationWrapper(registration);
        }

        public IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, DocumentSnapshotHandler listener)
        {
            var registration = DocumentReference.AddSnapshotListener(includeMetadataChanges, (snapshot, error) =>
            {
                listener?.Invoke(snapshot == null ? null : new DocumentSnapshotWrapper(snapshot),
                                 error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });

            return new ListenerRegistrationWrapper(registration);
        }
    }
}
