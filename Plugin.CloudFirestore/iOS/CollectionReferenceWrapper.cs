using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Firebase.CloudFirestore;
using Plugin.CloudFirestore;
using System.Reactive.Linq;
using CoreFoundation;
using ObjCRuntime;

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
            var query = _collectionReference.LimitedTo((nint)limit);
            return new QueryWrapper(query);
        }

        public IQuery LimitToLast(long limit)
        {
            var query = _collectionReference.LimitedToLast((nint)limit);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field)
        {
            var query = _collectionReference.OrderedBy(field);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(FieldPath field)
        {
            var query = _collectionReference.OrderedBy(field?.ToNative()!);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field, bool descending)
        {
            var query = _collectionReference.OrderedBy(field, descending);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(FieldPath field, bool descending)
        {
            var query = _collectionReference.OrderedBy(field?.ToNative()!, descending);
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo(string field, object? value)
        {
            var query = _collectionReference.WhereEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo(FieldPath field, object? value)
        {
            var query = _collectionReference.WhereEqualsTo(field?.ToNative()!, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThan(string field, object value)
        {
            var query = _collectionReference.WhereGreaterThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThan(FieldPath field, object value)
        {
            var query = _collectionReference.WhereGreaterThan(field?.ToNative()!, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThanOrEqualsTo(string field, object value)
        {
            var query = _collectionReference.WhereGreaterThanOrEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThanOrEqualsTo(FieldPath field, object value)
        {
            var query = _collectionReference.WhereGreaterThanOrEqualsTo(field?.ToNative()!, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThan(string field, object value)
        {
            var query = _collectionReference.WhereLessThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThan(FieldPath field, object value)
        {
            var query = _collectionReference.WhereLessThan(field?.ToNative()!, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThanOrEqualsTo(string field, object value)
        {
            var query = _collectionReference.WhereLessThanOrEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThanOrEqualsTo(FieldPath field, object value)
        {
            var query = _collectionReference.WhereLessThanOrEqualsTo(field?.ToNative()!, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereArrayContains(string field, object value)
        {
            var query = _collectionReference.WhereArrayContains(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereArrayContains(FieldPath field, object value)
        {
            var query = _collectionReference.WhereArrayContains(field?.ToNative()!, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereArrayContainsAny(string field, IEnumerable<object> values)
        {
            var query = _collectionReference.WhereArrayContainsAny(field, values?.Select(x => x.ToNativeFieldValue()).ToArray()!);
            return new QueryWrapper(query);
        }

        public IQuery WhereArrayContainsAny(FieldPath field, IEnumerable<object> values)
        {
            var query = _collectionReference.WhereArrayContainsAny(field?.ToNative()!, values?.Select(x => x.ToNativeFieldValue()).ToArray()!);
            return new QueryWrapper(query);
        }

        public IQuery WhereIn(string field, IEnumerable<object> values)
        {
            var query = _collectionReference.WhereFieldIn(field, values?.Select(x => x.ToNativeFieldValue()).ToArray()!);
            return new QueryWrapper(query);
        }

        public IQuery WhereIn(FieldPath field, IEnumerable<object> values)
        {
            var query = _collectionReference.WhereFieldIn(field?.ToNative()!, values?.Select(x => x.ToNativeFieldValue()).ToArray()!);
            return new QueryWrapper(query);
        }

        public IQuery WhereNotEqualTo(string field, object value)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var fieldHandle = CFString.CreateNative(field);
            var valueHandle = NSObject.FromObject(value.ToNativeFieldValue()).GetNonNullHandle(nameof(value));

            var query = Runtime.GetNSObject<Query>(Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr(_collectionReference.Handle, Selector.GetHandle("queryWhereField:isNotEqualTo:"), fieldHandle, valueHandle));
            CFString.ReleaseNative(fieldHandle);
            return new QueryWrapper(query!);
        }

        public IQuery WhereNotEqualTo(FieldPath field, object value)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var fieldHandle = field.ToNative().GetNonNullHandle(nameof(field));
            var valueHandle = NSObject.FromObject(value.ToNativeFieldValue()).GetNonNullHandle(nameof(value));

            var query = Runtime.GetNSObject<Query>(Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr(_collectionReference.Handle, Selector.GetHandle("queryWhereFieldPath:isNotEqualTo:"), fieldHandle, valueHandle));
            return new QueryWrapper(query!);
        }

        public IQuery WhereNotIn(string field, IEnumerable<object> values)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var fieldHandle = CFString.CreateNative(field);
            var nsArray = NSArray.FromNSObjects(values.Select(x => NSObject.FromObject(x.ToNativeFieldValue())).ToArray());

            var query = Runtime.GetNSObject<Query>(Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr(_collectionReference.Handle, Selector.GetHandle("queryWhereField:notIn:"), fieldHandle, nsArray.Handle));
            CFString.ReleaseNative(fieldHandle);
            nsArray.Dispose();
            return new QueryWrapper(query!);
        }

        public IQuery WhereNotIn(FieldPath field, IEnumerable<object> values)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var fieldHandle = field.ToNative().GetNonNullHandle(nameof(field));
            var nsArray = NSArray.FromNSObjects(values.Select(x => NSObject.FromObject(x.ToNativeFieldValue())).ToArray());

            var query = Runtime.GetNSObject<Query>(Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr(_collectionReference.Handle, Selector.GetHandle("queryWhereFieldPath:notIn:"), fieldHandle, nsArray.Handle));
            nsArray.Dispose();
            return new QueryWrapper(query!);
        }

        public IQuery StartAt(IDocumentSnapshot document)
        {
            var query = _collectionReference.StartingAt(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery StartAt(params object?[] fieldValues)
        {
            var query = _collectionReference.StartingAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(IDocumentSnapshot document)
        {
            var query = _collectionReference.StartingAfter(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(params object?[] fieldValues)
        {
            var query = _collectionReference.StartingAfter(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(IDocumentSnapshot document)
        {
            var query = _collectionReference.EndingAt(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(params object?[] fieldValues)
        {
            var query = _collectionReference.EndingAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(IDocumentSnapshot document)
        {
            var query = _collectionReference.EndingBefore(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(params object?[] fieldValues)
        {
            var query = _collectionReference.EndingBefore(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IDocumentReference CreateDocument()
        {
            return Document();
        }

        public IDocumentReference Document()
        {
            var doccuntReference = _collectionReference.CreateDocument();
            return new DocumentReferenceWrapper(doccuntReference);
        }

        public IDocumentReference GetDocument(string documentPath)
        {
            return Document(documentPath);
        }

        public IDocumentReference Document(string documentPath)
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

        public void GetDocuments(Source source, QuerySnapshotHandler handler)
        {
            _collectionReference.GetDocuments(source.ToNative(), (snapshot, error) =>
            {
                handler?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                error == null ? null : ExceptionMapper.Map(error));
            });
        }

        public Task<IQuerySnapshot> GetDocumentsAsync()
        {
            return GetAsync();
        }

        public Task<IQuerySnapshot> GetAsync()
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
                    tcs.SetResult(new QuerySnapshotWrapper(snapshot!));
                }
            });

            return tcs.Task;
        }

        public Task<IQuerySnapshot> GetDocumentsAsync(Source source)
        {
            return GetAsync(source);
        }

        public Task<IQuerySnapshot> GetAsync(Source source)
        {
            var tcs = new TaskCompletionSource<IQuerySnapshot>();

            _collectionReference.GetDocuments(source.ToNative(), (snapshot, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
                }
                else
                {
                    tcs.SetResult(new QuerySnapshotWrapper(snapshot!));
                }
            });

            return tcs.Task;
        }

        public void AddDocument(object data, CompletionHandler handler)
        {
            _collectionReference.AddDocument(data.ToNativeFieldValues<object, NSString>()!, (error) =>
            {
                handler?.Invoke(error == null ? null : ExceptionMapper.Map(error));
            });
        }

        public Task AddDocumentAsync(object data)
        {
            var tcs = new TaskCompletionSource<bool>();

            _collectionReference.AddDocument(data.ToNativeFieldValues<object, NSString>()!, (error) =>
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

        public Task<IDocumentReference> AddAsync<T>(T data)
        {
            var tcs = new TaskCompletionSource<IDocumentReference>();

            DocumentReference? document = null;
            document = _collectionReference.AddDocument(data.ToNativeFieldValues<T, NSString>()!, (error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(error));
                }
                else
                {
                    tcs.SetResult(new DocumentReferenceWrapper(document!));
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
