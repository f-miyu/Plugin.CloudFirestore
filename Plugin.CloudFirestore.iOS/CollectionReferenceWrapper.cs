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
        public string Id => _collectionReference.Id;

        public string Path => _collectionReference.Path;

        public IDocumentReference Parent => _collectionReference.Parent == null ? null : new DocumentReferenceWrapper(_collectionReference.Parent);

        private readonly CollectionReference _collectionReference;

        public CollectionReferenceWrapper(CollectionReference collectionReference)
        {
            _collectionReference = collectionReference;
        }

        public IQuery LimitTo(int limit)
        {
            var query = _collectionReference.LimitedTo(limit);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field, bool descending)
        {
            var query = _collectionReference.OrderedBy(field, descending);
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo<T>(string field, T value)
        {
            var query = _collectionReference.WhereEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThan<T>(string field, T value)
        {
            var query = _collectionReference.WhereGreaterThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThanOrEqualsTo<T>(string field, T value)
        {
            var query = _collectionReference.WhereGreaterThanOrEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThan<T>(string field, T value)
        {
            var query = _collectionReference.WhereLessThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThanOrEqualsTo<T>(string field, T value)
        {
            var query = _collectionReference.WhereLessThanOrEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery StartAt(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = _collectionReference.StartingAt((DocumentSnapshot)wrapper);
            return new QueryWrapper(query);
        }

        public IQuery StartAt<T>(IEnumerable<T> fieldValues)
        {
            var query = _collectionReference.StartingAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = _collectionReference.StartingAfter((DocumentSnapshot)wrapper);
            return new QueryWrapper(query);
        }

        public IQuery StartAfter<T>(IEnumerable<T> fieldValues)
        {
            var query = _collectionReference.StartingAfter(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = _collectionReference.EndingAt((DocumentSnapshot)wrapper);
            return new QueryWrapper(query);
        }

        public IQuery EndAt<T>(IEnumerable<T> fieldValues)
        {
            var query = _collectionReference.EndingAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = _collectionReference.EndingBefore((DocumentSnapshot)wrapper);
            return new QueryWrapper(query);
        }

        public IQuery EndBefore<T>(IEnumerable<T> fieldValues)
        {
            var query = _collectionReference.EndingBefore(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IDocumentReference GetDocument(string documentPath)
        {
            var doccuntReference = _collectionReference.GetDocument(documentPath);
            return new DocumentReferenceWrapper(doccuntReference);
        }

        public void GetDocuments(QuerySnapshotHandler handler)
        {
            _collectionReference.GetDocuments((snapshot, error) =>
            {
                handler?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                error == null ? null : ExceptionMapper.Map(error));
            });
        }

        public Task<IQuerySnapshot> GetDocumentsAsync()
        {
            var tcs = new TaskCompletionSource<IQuerySnapshot>();

            _collectionReference.GetDocuments((snapshot, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
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
            _collectionReference.AddDocument(data.ToNativeFieldValues(), (error) =>
            {
                handler?.Invoke(error == null ? null : ExceptionMapper.Map(error));
            });
        }

        public Task AddDocumentAsync<T>(T data) where T : class
        {
            var tcs = new TaskCompletionSource<bool>();

            _collectionReference.AddDocument(data.ToNativeFieldValues(), (error) =>
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

        public IListenerRegistration AddSnapshotListener(QuerySnapshotHandler listener)
        {
            var registration = _collectionReference.AddSnapshotListener((snapshot, error) =>
            {
                listener?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                 error == null ? null : ExceptionMapper.Map(error));
            });

            return new ListenerRegistrationWrapper(registration);
        }

        public IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, QuerySnapshotHandler listener)
        {
            var registration = _collectionReference.AddSnapshotListener(includeMetadataChanges, (snapshot, error) =>
            {
                listener?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                 error == null ? null : ExceptionMapper.Map(error));
            });

            return new ListenerRegistrationWrapper(registration);
        }
    }
}
