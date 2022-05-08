using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Android.Runtime;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public class CollectionReferenceWrapper : ICollectionReference, IEquatable<CollectionReferenceWrapper>
    {
        private readonly CollectionReference _collectionReference;

        public CollectionReferenceWrapper(CollectionReference collectionReference)
        {
            _collectionReference = collectionReference ?? throw new ArgumentNullException(nameof(collectionReference));
        }

        public string Id => _collectionReference.Id;

        public string Path => _collectionReference.Path;

        public IDocumentReference? Parent => _collectionReference.Parent == null ? null : new DocumentReferenceWrapper(_collectionReference.Parent);

        public IFirestore Firestore => FirestoreProvider.GetFirestore(_collectionReference.Firestore);

        public IQuery LimitTo(long limit)
        {
            var query = _collectionReference.Limit(limit);
            return new QueryWrapper(query);
        }

        public IQuery LimitToLast(long limit)
        {
            var query = _collectionReference.LimitToLast(limit);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field)
        {
            var query = _collectionReference.OrderBy(field);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(FieldPath field)
        {
            var query = _collectionReference.OrderBy(field?.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field, bool descending)
        {
            var direction = descending ? Query.Direction.Descending : Query.Direction.Ascending;

            var query = _collectionReference.OrderBy(field, direction);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(FieldPath field, bool descending)
        {
            var direction = descending ? Query.Direction.Descending : Query.Direction.Ascending;

            var query = _collectionReference.OrderBy(field?.ToNative(), direction);
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo(string field, object? value)
        {
            var query = _collectionReference.WhereEqualTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo(FieldPath field, object? value)
        {
            var query = _collectionReference.WhereEqualTo(field?.ToNative(), value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThan(string field, object value)
        {
            var query = _collectionReference.WhereGreaterThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThan(FieldPath field, object value)
        {
            var query = _collectionReference.WhereGreaterThan(field?.ToNative(), value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThanOrEqualsTo(string field, object value)
        {
            var query = _collectionReference.WhereGreaterThanOrEqualTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThanOrEqualsTo(FieldPath field, object value)
        {
            var query = _collectionReference.WhereGreaterThanOrEqualTo(field?.ToNative(), value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThan(string field, object value)
        {
            var query = _collectionReference.WhereLessThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThan(FieldPath field, object value)
        {
            var query = _collectionReference.WhereLessThan(field?.ToNative(), value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThanOrEqualsTo(string field, object value)
        {
            var query = _collectionReference.WhereLessThanOrEqualTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThanOrEqualsTo(FieldPath field, object value)
        {
            var query = _collectionReference.WhereLessThanOrEqualTo(field?.ToNative(), value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereArrayContains(string field, object value)
        {
            var query = _collectionReference.WhereArrayContains(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereArrayContains(FieldPath field, object value)
        {
            var query = _collectionReference.WhereArrayContains(field?.ToNative(), value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereArrayContainsAny(string field, IEnumerable<object> values)
        {
            var query = _collectionReference.WhereArrayContainsAny(field, values?.Select(x => x.ToNativeFieldValue()).ToList());
            return new QueryWrapper(query);
        }

        public IQuery WhereArrayContainsAny(FieldPath field, IEnumerable<object> values)
        {
            var query = _collectionReference.WhereArrayContainsAny(field?.ToNative(), values?.Select(x => x.ToNativeFieldValue()).ToList());
            return new QueryWrapper(query);
        }

        public IQuery WhereIn(string field, IEnumerable<object> values)
        {
            var query = _collectionReference.WhereIn(field, values?.Select(x => x.ToNativeFieldValue()).ToList());
            return new QueryWrapper(query);
        }

        public IQuery WhereIn(FieldPath field, IEnumerable<object> values)
        {
            var query = _collectionReference.WhereIn(field?.ToNative(), values?.Select(x => x.ToNativeFieldValue()).ToList());
            return new QueryWrapper(query);
        }

        public IQuery WhereNotEqualTo(string field, object value)
        {
            var query = _collectionReference.WhereNotEqualTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereNotEqualTo(FieldPath field, object value)
        {
            var query = _collectionReference.WhereNotEqualTo(field?.ToNative(), value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereNotIn(string field, IEnumerable<object> values)
        {
            var query = _collectionReference.WhereNotIn(field, values?.Select(x => x.ToNativeFieldValue()).ToList());
            return new QueryWrapper(query);
        }

        public IQuery WhereNotIn(FieldPath field, IEnumerable<object> values)
        {
            var query = _collectionReference.WhereNotIn(field?.ToNative(), values?.Select(x => x.ToNativeFieldValue()).ToList());
            return new QueryWrapper(query);
        }

        public IQuery StartAt(IDocumentSnapshot document)
        {
            var query = _collectionReference.StartAt(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery StartAt(params object?[] fieldValues)
        {
            var query = _collectionReference.StartAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(IDocumentSnapshot document)
        {
            var query = _collectionReference.StartAfter(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(params object?[] fieldValues)
        {
            var query = _collectionReference.StartAfter(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(IDocumentSnapshot document)
        {
            var query = _collectionReference.EndAt(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(params object?[] fieldValues)
        {
            var query = _collectionReference.EndAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(IDocumentSnapshot document)
        {
            var query = _collectionReference.EndBefore(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(params object?[] fieldValues)
        {
            var query = _collectionReference.EndBefore(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IDocumentReference CreateDocument()
        {
            return Document();
        }

        public IDocumentReference Document()
        {
            var doccuntReference = _collectionReference.Document();
            return new DocumentReferenceWrapper(doccuntReference);
        }

        public IDocumentReference GetDocument(string documentPath)
        {
            return Document(documentPath);
        }

        public IDocumentReference Document(string documentPath)
        {
            var doccuntReference = _collectionReference.Document(documentPath);
            return new DocumentReferenceWrapper(doccuntReference);
        }

        public void GetDocuments(QuerySnapshotHandler handler)
        {
            _collectionReference.Get().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                var snapshot = !task.IsSuccessful ? null : task.Result.JavaCast<QuerySnapshot>();
                handler?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public void GetDocuments(Source source, QuerySnapshotHandler handler)
        {
            _collectionReference.Get(source.ToNative()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                var snapshot = !task.IsSuccessful ? null : task.Result.JavaCast<QuerySnapshot>();
                handler?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public Task<IQuerySnapshot> GetDocumentsAsync()
        {
            return GetAsync();
        }

        public Task<IQuerySnapshot> GetAsync()
        {
            var tcs = new TaskCompletionSource<IQuerySnapshot>();

            _collectionReference.Get().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                if (task.IsSuccessful)
                {
                    var snapshot = task.Result.JavaCast<QuerySnapshot>();
                    tcs.SetResult(new QuerySnapshotWrapper(snapshot!));
                }
                else
                {
                    tcs.SetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            return tcs.Task;
        }

        public Task<IQuerySnapshot> GetDocumentsAsync(Source source)
        {
            return GetAsync(source);
        }

        public Task<IQuerySnapshot> GetAsync(Source source)
        {
            var tcs = new TaskCompletionSource<IQuerySnapshot>();

            _collectionReference.Get(source.ToNative()).AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                if (task.IsSuccessful)
                {
                    var snapshot = task.Result.JavaCast<QuerySnapshot>();
                    tcs.SetResult(new QuerySnapshotWrapper(snapshot!));
                }
                else
                {
                    tcs.SetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            return tcs.Task;
        }

        public void AddDocument(object data, CompletionHandler handler)
        {
            _collectionReference.Add(data.ToNativeFieldValues())
                                .AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
                                {
                                    handler?.Invoke(task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
                                }));
        }

        public Task AddDocumentAsync(object data)
        {
            var tcs = new TaskCompletionSource<bool>();

            _collectionReference.Add(data.ToNativeFieldValues())
                                .AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
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

        public Task<IDocumentReference> AddAsync<T>(T data)
        {
            var tcs = new TaskCompletionSource<IDocumentReference>();

            _collectionReference.Add(data.ToNativeFieldValues())
                                .AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
                                {
                                    if (task.IsSuccessful)
                                    {
                                        var document = task.Result.JavaCast<DocumentReference>();
                                        tcs.SetResult(new DocumentReferenceWrapper(document!));
                                    }
                                    else
                                    {
                                        tcs.SetException(ExceptionMapper.Map(task.Exception));
                                    }
                                }));

            return tcs.Task;
        }

        public IListenerRegistration AddSnapshotListener(QuerySnapshotHandler listener)
        {
            var registration = _collectionReference.AddSnapshotListener(new EventHandlerListener<QuerySnapshot>((value, error) =>
            {
                listener?.Invoke(value == null ? null : new QuerySnapshotWrapper(value),
                                 error == null ? null : ExceptionMapper.Map(error));
            }));

            return new ListenerRegistrationWrapper(registration);
        }

        public IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, QuerySnapshotHandler listener)
        {
            var registration = _collectionReference.AddSnapshotListener(includeMetadataChanges ? MetadataChanges.Include : MetadataChanges.Exclude, new EventHandlerListener<QuerySnapshot>((value, error) =>
            {
                listener?.Invoke(value == null ? null : new QuerySnapshotWrapper(value),
                                 error == null ? null : ExceptionMapper.Map(error));
            }));

            return new ListenerRegistrationWrapper(registration);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CollectionReferenceWrapper);
        }

        public bool Equals(CollectionReferenceWrapper? other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_collectionReference, other._collectionReference)) return true;
            return _collectionReference.Equals(other._collectionReference);
        }

        public override int GetHashCode()
        {
            return _collectionReference?.GetHashCode() ?? 0;
        }
    }
}
