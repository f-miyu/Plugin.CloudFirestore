using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Firebase.CloudFirestore;
using Plugin.CloudFirestore;
using System.Reactive.Linq;

namespace Plugin.CloudFirestore
{
    public class CollectionReferenceWrapper : ICollectionReference
    {
        public CollectionReference CollectionReference { get; }

        public CollectionReferenceWrapper(CollectionReference collectionReference)
        {
            CollectionReference = collectionReference;
        }

        public IQuery LimitTo(int limit)
        {
            var query = CollectionReference.LimitedTo(limit);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field, bool descending)
        {
            var query = CollectionReference.OrderedBy(field, descending);
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo<T>(string field, T value)
        {
            var query = CollectionReference.WhereEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThan<T>(string field, T value)
        {
            var query = CollectionReference.WhereGreaterThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThanOrEqualsTo<T>(string field, T value)
        {
            var query = CollectionReference.WhereGreaterThanOrEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThan<T>(string field, T value)
        {
            var query = CollectionReference.WhereLessThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThanOrEqualsTo<T>(string field, T value)
        {
            var query = CollectionReference.WhereLessThanOrEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery StartAt(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = CollectionReference.StartingAt(wrapper.DocumentSnapshot);
            return new QueryWrapper(query);
        }

        public IQuery StartAt<T>(IEnumerable<T> fieldValues)
        {
            var query = CollectionReference.StartingAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = CollectionReference.StartingAfter(wrapper.DocumentSnapshot);
            return new QueryWrapper(query);
        }

        public IQuery StartAfter<T>(IEnumerable<T> fieldValues)
        {
            var query = CollectionReference.StartingAfter(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = CollectionReference.EndingAt(wrapper.DocumentSnapshot);
            return new QueryWrapper(query);
        }

        public IQuery EndAt<T>(IEnumerable<T> fieldValues)
        {
            var query = CollectionReference.EndingAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = CollectionReference.EndingBefore(wrapper.DocumentSnapshot);
            return new QueryWrapper(query);
        }

        public IQuery EndBefore<T>(IEnumerable<T> fieldValues)
        {
            var query = CollectionReference.EndingBefore(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IDocumentReference GetDocument(string documentPath)
        {
            var doccuntReference = CollectionReference.GetDocument(documentPath);
            return new DocumentReferenceWrapper(doccuntReference);
        }

        public void GetDocuments(QuerySnapshotHandler handler)
        {
            CollectionReference.GetDocuments((snapshot, error) =>
            {
                handler?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });
        }

        public Task<IQuerySnapshot> GetDocumentsAsync()
        {
            var tcs = new TaskCompletionSource<IQuerySnapshot>();

            CollectionReference.GetDocuments((snapshot, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(new CloudFirestoreException(error.LocalizedDescription));
                }
                else
                {
                    tcs.SetResult(snapshot == null ? null : new QuerySnapshotWrapper(snapshot));
                }
            });

            return tcs.Task;
        }

        public void AddDocument<T>(T data, CompletionHandler handler) where T : class
        {
            CollectionReference.AddDocument(data.ToNativeFieldValues(), (error) =>
            {
                handler?.Invoke(error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });
        }

        public Task AddDocumentAsync<T>(T data) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            CollectionReference.AddDocument(data.ToNativeFieldValues(), (error) =>
            {
                if (error != null)
                {
                    tcs.SetException(new NSErrorException(error));
                }
                else
                {
                    tcs.SetResult(true);
                }
            });

            return tcs.Task;
        }

        public IListenerRegistration AddSnapshotListener(QuerySnapshotHandler listener)
        {
            var registration = CollectionReference.AddSnapshotListener((snapshot, error) =>
            {
                listener?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                 error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });

            return new ListenerRegistrationWrapper(registration);
        }

        public IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, QuerySnapshotHandler listener)
        {
            var registration = CollectionReference.AddSnapshotListener(includeMetadataChanges, (snapshot, error) =>
            {
                listener?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                 error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });

            return new ListenerRegistrationWrapper(registration);
        }
    }
}
