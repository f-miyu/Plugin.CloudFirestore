using System;
using System.Threading.Tasks;
using Android.Runtime;
using Firebase.Firestore;
using System.Reactive.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.CloudFirestore
{
    public class DocumentReferenceWrapper : IDocumentReference
    {
        internal DocumentReference DocumentReference { get; }

        public DocumentReferenceWrapper(DocumentReference documentReference)
        {
            DocumentReference = documentReference;
        }

        public ICollectionReference GetCollection(string collectionPath)
        {
            var collectionReference = DocumentReference.Collection(collectionPath);
            return new CollectionReferenceWrapper(collectionReference);
        }

        public void GetDocument(DocumentSnapshotHandler handler)
        {
            var tcs = new TaskCompletionSource<IDocumentSnapshot>();

            DocumentReference.Get().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                var snapshot = !task.IsSuccessful ? null : task.Result.JavaCast<DocumentSnapshot>();
                handler?.Invoke(snapshot == null ? null : new DocumentSnapshotWrapper(snapshot),
                                task.IsSuccessful ? null : new CloudFirestoreException(task.Exception.Message));
            }));
        }

        public Task<IDocumentSnapshot> GetDocumentAsync()
        {
            var tcs = new TaskCompletionSource<IDocumentSnapshot>();

            DocumentReference.Get().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                if (task.IsSuccessful)
                {
                    var snapshot = task.Result.JavaCast<DocumentSnapshot>();
                    tcs.SetResult(snapshot == null ? null : new DocumentSnapshotWrapper(snapshot));
                }
                else
                {
                    tcs.SetException(new CloudFirestoreException(task.Exception.Message));
                }
            }));

            return tcs.Task;
        }

        public void SetData<T>(T documentData, CompletionHandler handler) where T : class
        {
            DocumentReference.Set(documentData.ToNativeFieldValues()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : new CloudFirestoreException(task.Exception.Message));
            }));
        }

        public Task SetDataAsync<T>(T documentData) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            DocumentReference.Set(documentData.ToNativeFieldValues()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public void SetData<T>(T documentData, IEnumerable<string> mergeFields, CompletionHandler handler) where T : class
        {
            DocumentReference.Set(documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields.ToArray())).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : new CloudFirestoreException(task.Exception.Message));
            }));
        }

        public Task SetDataAsync<T>(T documentData, IEnumerable<string> mergeFields) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            DocumentReference.Set(documentData.ToNativeFieldValues(), SetOptions.MergeFields(mergeFields.ToArray())).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public void SetData<T>(T documentData, bool merge, CompletionHandler handler) where T : class
        {
            if (!merge)
            {
                SetData(documentData, handler);
                return;
            }

            DocumentReference.Set(documentData.ToNativeFieldValues(), SetOptions.Merge()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : new CloudFirestoreException(task.Exception.Message));
            }));
        }

        public Task SetDataAsync<T>(T documentData, bool merge) where T : class
        {
            if (!merge)
            {
                return SetDataAsync(documentData);
            }

            var tcs = new TaskCompletionSource<bool>();

            DocumentReference.Set(documentData.ToNativeFieldValues(), SetOptions.Merge()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public void UpdateData<T>(T fields, CompletionHandler handler) where T : class
        {
            DocumentReference.Update(fields.ToNativeFieldValues()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : new CloudFirestoreException(task.Exception.Message));
            }));
        }

        public Task UpdateDataAsync<T>(T fields) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            DocumentReference.Update(fields.ToNativeFieldValues()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public void UpdateData<T>(string field, T value, CompletionHandler handler, params object[] moreFieldsAndValues)
        {
            DocumentReference.Update(field, value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : new CloudFirestoreException(task.Exception.Message));
            }));
        }

        public Task UpdateDataAsync<T>(string field, T value, params object[] moreFieldsAndValues)
        {
            var tcs = new TaskCompletionSource<bool>();

            DocumentReference.Update(field, value.ToNativeFieldValue(), moreFieldsAndValues.Select(x => x.ToNativeFieldValue()).ToArray()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public void DeleteDocument(CompletionHandler handler)
        {
            DocumentReference.Delete().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : new CloudFirestoreException(task.Exception.Message));
            }));
        }

        public Task DeleteDocumentAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            DocumentReference.Delete().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public IListenerRegistration AddSnapshotListener(DocumentSnapshotHandler listener)
        {
            var registration = DocumentReference.AddSnapshotListener(new EventHandlerListener<DocumentSnapshot>((value, error) =>
            {
                listener?.Invoke(value == null ? null : new DocumentSnapshotWrapper(value),
                                 error == null ? null : new CloudFirestoreException(error.Message));
            }));

            return new ListenerRegistrationWrapper(registration);
        }

        public IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, DocumentSnapshotHandler listener)
        {
            if (!includeMetadataChanges)
            {
                return AddSnapshotListener(listener);
            }

            var option = new DocumentListenOptions().IncludeMetadataChanges();

            var registration = DocumentReference.AddSnapshotListener(option, new EventHandlerListener<DocumentSnapshot>((value, error) =>
            {
                listener?.Invoke(value == null ? null : new DocumentSnapshotWrapper(value),
                                 error == null ? null : new CloudFirestoreException(error.Message));
            }));

            return new ListenerRegistrationWrapper(registration);
        }
    }
}
