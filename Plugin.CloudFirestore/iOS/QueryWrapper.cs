using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using System.Collections.Generic;
using System.Linq;
using ObjCRuntime;
using Foundation;
using System.Reflection;
using CoreFoundation;

namespace Plugin.CloudFirestore
{
    public class QueryWrapper : IQuery, IEquatable<QueryWrapper>
    {
        private readonly Query _query;

        public QueryWrapper(Query query)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }

        public IFirestore Firestore => FirestoreProvider.GetFirestore(_query.Firestore);

        public IQuery LimitTo(long limit)
        {
            var query = _query.LimitedTo((nint)limit);
            return new QueryWrapper(query);
        }

        public IQuery LimitToLast(long limit)
        {
            var query = _query.LimitedToLast((nint)limit);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field)
        {
            var query = _query.OrderedBy(field);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(FieldPath field)
        {
            var query = _query.OrderedBy(field?.ToNative()!);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field, bool descending)
        {
            var query = _query.OrderedBy(field, descending);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(FieldPath field, bool descending)
        {
            var query = _query.OrderedBy(field?.ToNative()!, descending);
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo(string field, object? value)
        {
            var query = _query.WhereEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo(FieldPath field, object? value)
        {
            var query = _query.WhereEqualsTo(field?.ToNative()!, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThan(string field, object value)
        {
            var query = _query.WhereGreaterThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThan(FieldPath field, object value)
        {
            var query = _query.WhereGreaterThan(field?.ToNative()!, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThanOrEqualsTo(string field, object value)
        {
            var query = _query.WhereGreaterThanOrEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThanOrEqualsTo(FieldPath field, object value)
        {
            var query = _query.WhereGreaterThanOrEqualsTo(field?.ToNative()!, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThan(string field, object value)
        {
            var query = _query.WhereLessThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThan(FieldPath field, object value)
        {
            var query = _query.WhereLessThan(field?.ToNative()!, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThanOrEqualsTo(string field, object value)
        {
            var query = _query.WhereLessThanOrEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThanOrEqualsTo(FieldPath field, object value)
        {
            var query = _query.WhereLessThanOrEqualsTo(field?.ToNative()!, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereArrayContains(string field, object value)
        {
            var query = _query.WhereArrayContains(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereArrayContains(FieldPath field, object value)
        {
            var query = _query.WhereArrayContains(field?.ToNative()!, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereArrayContainsAny(string field, IEnumerable<object> values)
        {
            var query = _query.WhereArrayContains(field, values?.Select(x => x.ToNativeFieldValue()).ToArray()!);
            return new QueryWrapper(query);
        }

        public IQuery WhereArrayContainsAny(FieldPath field, IEnumerable<object> values)
        {
            var query = _query.WhereArrayContains(field?.ToNative()!, values?.Select(x => x.ToNativeFieldValue()).ToArray()!);
            return new QueryWrapper(query);
        }

        public IQuery WhereIn(string field, IEnumerable<object> values)
        {
            var query = _query.WhereFieldIn(field, values?.Select(x => x.ToNativeFieldValue()).ToArray()!);
            return new QueryWrapper(query);
        }

        public IQuery WhereIn(FieldPath field, IEnumerable<object> values)
        {
            var query = _query.WhereFieldIn(field?.ToNative()!, values?.Select(x => x.ToNativeFieldValue()).ToArray()!);
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

            var query = Runtime.GetNSObject<Query>(Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr(_query.Handle, Selector.GetHandle("queryWhereField:isNotEqualTo:"), fieldHandle, valueHandle));
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

            var query = Runtime.GetNSObject<Query>(Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr(_query.Handle, Selector.GetHandle("queryWhereFieldPath:isNotEqualTo:"), fieldHandle, valueHandle));
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

            var query = Runtime.GetNSObject<Query>(Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr(_query.Handle, Selector.GetHandle("queryWhereField:notIn:"), fieldHandle, nsArray.Handle));
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

            var query = Runtime.GetNSObject<Query>(Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr(_query.Handle, Selector.GetHandle("queryWhereFieldPath:notIn:"), fieldHandle, nsArray.Handle));
            nsArray.Dispose();
            return new QueryWrapper(query!);
        }

        public IQuery StartAt(IDocumentSnapshot document)
        {
            var query = _query.StartingAt(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery StartAt(params object?[] fieldValues)
        {
            var query = _query.StartingAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(IDocumentSnapshot document)
        {
            var query = _query.StartingAfter(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(params object?[] fieldValues)
        {
            var query = _query.StartingAfter(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(IDocumentSnapshot document)
        {
            var query = _query.EndingAt(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(params object?[] fieldValues)
        {
            var query = _query.EndingAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(IDocumentSnapshot document)
        {
            var query = _query.EndingBefore(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(params object?[] fieldValues)
        {
            var query = _query.EndingBefore(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public void GetDocuments(QuerySnapshotHandler handler)
        {
            _query.GetDocuments((snapshot, error) =>
            {
                handler?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                error == null ? null : ExceptionMapper.Map(error));
            });
        }

        public void GetDocuments(Source source, QuerySnapshotHandler handler)
        {
            _query.GetDocuments(source.ToNative(), (snapshot, error) =>
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

            _query.GetDocuments((snapshot, error) =>
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

            _query.GetDocuments(source.ToNative(), (snapshot, error) =>
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

        public IListenerRegistration AddSnapshotListener(QuerySnapshotHandler listener)
        {
            var registration = _query.AddSnapshotListener((snapshot, error) =>
            {
                listener?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                 error == null ? null : ExceptionMapper.Map(error));
            });

            return new ListenerRegistrationWrapper(registration);
        }

        public IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, QuerySnapshotHandler listener)
        {
            var registration = _query.AddSnapshotListener(includeMetadataChanges, (snapshot, error) =>
            {
                listener?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                 error == null ? null : ExceptionMapper.Map(error));
            });

            return new ListenerRegistrationWrapper(registration);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as QueryWrapper);
        }

        public bool Equals(QueryWrapper? other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_query, other._query)) return true;
            return _query.Equals(other._query);
        }

        public override int GetHashCode()
        {
            return _query?.GetHashCode() ?? 0;
        }
    }
}
