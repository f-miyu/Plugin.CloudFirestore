using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public class QueryWrapper : IQuery, IEquatable<QueryWrapper>
    {
        private readonly Query _query;

        public QueryWrapper(Query query)
        {
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }

        public IFirestore Firestore => FirestoreProvider.GetFirestore(_query.Database);

        public IQuery LimitTo(long limit)
        {
            var query = _query.Limit((int)limit);
            return new QueryWrapper(query);
        }

        public IQuery LimitToLast(long limit)
        {
            var query = _query.LimitToLast((int)limit);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field)
        {
            var query = _query.OrderBy(field);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(FieldPath field)
        {
            var query = _query.OrderBy(field?.ToNative()!);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field, bool descending)
        {
            Google.Cloud.Firestore.Query query;
            if (descending)
            {
                query = _query.OrderByDescending(field);
            }
            else
            {
                query = _query.OrderBy(field);
            }
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(FieldPath field, bool descending)
        {
            Google.Cloud.Firestore.Query query;
            if (descending)
            {
                query = _query.OrderByDescending(field?.ToNative()!);
            }
            else
            {
                query = _query.OrderBy(field?.ToNative()!);
            }
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo(string field, object? value)
        {
            var query = _query.WhereEqualTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo(FieldPath field, object? value)
        {
            var query = _query.WhereEqualTo(field?.ToNative()!, value.ToNativeFieldValue());
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
            var query = _query.WhereGreaterThanOrEqualTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThanOrEqualsTo(FieldPath field, object value)
        {
            var query = _query.WhereGreaterThanOrEqualTo(field?.ToNative()!, value.ToNativeFieldValue());
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
            var query = _query.WhereGreaterThanOrEqualTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThanOrEqualsTo(FieldPath field, object value)
        {
            var query = _query.WhereGreaterThanOrEqualTo(field?.ToNative()!, value.ToNativeFieldValue());
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
            var query = _query.WhereIn(field, values?.Select(x => x.ToNativeFieldValue()).ToArray()!);
            return new QueryWrapper(query);
        }

        public IQuery WhereIn(FieldPath field, IEnumerable<object> values)
        {
            var query = _query.WhereIn(field?.ToNative()!, values?.Select(x => x.ToNativeFieldValue()).ToArray()!);
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

            var fieldHandle = field;
            var valueHandle = value.ToNativeFieldValue();
            var query = _query.WhereNotEqualTo(fieldHandle, valueHandle);
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

            var fieldHandle = field.ToNative();
            var valueHandle = value.ToNativeFieldValue();
            var query = _query.WhereNotEqualTo(fieldHandle, valueHandle);
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

            var fieldHandle = field;
            var nsArray = values.Select(x => x.ToNativeFieldValue()).ToArray();
            var query = _query.WhereNotIn(fieldHandle, nsArray);
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

            var fieldHandle = field.ToNative();
            var nsArray = values.Select(x => x.ToNativeFieldValue()).ToArray();
            var query = _query.WhereNotIn(fieldHandle, nsArray);
            return new QueryWrapper(query!);
        }

        public IQuery StartAt(IDocumentSnapshot document)
        {
            var query = _query.StartAt(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery StartAt(params object?[] fieldValues)
        {
            var query = _query.StartAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(IDocumentSnapshot document)
        {
            var query = _query.StartAfter(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(params object?[] fieldValues)
        {
            var query = _query.StartAfter(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(IDocumentSnapshot document)
        {
            var query = _query.EndAt(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(params object?[] fieldValues)
        {
            var query = _query.EndAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(IDocumentSnapshot document)
        {
            var query = _query.EndBefore(document.ToNative());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(params object?[] fieldValues)
        {
            var query = _query.EndBefore(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public void GetDocuments(QuerySnapshotHandler handler)
        {
            throw new NotImplementedException();
            //_query.GetDocuments((snapshot, error) =>
            //{
            //    handler?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
            //                    error == null ? null : ExceptionMapper.Map(error));
            //});
        }

        public void GetDocuments(Source source, QuerySnapshotHandler handler)
        {
            throw new NotImplementedException();
            //_query.GetDocuments(source.ToNative(), (snapshot, error) =>
            //{
            //    handler?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
            //                    error == null ? null : ExceptionMapper.Map(error));
            //});
        }

        public Task<IQuerySnapshot> GetDocumentsAsync()
        {
            return GetAsync();
        }

        public Task<IQuerySnapshot> GetAsync()
        {
            throw new NotImplementedException();
            //var tcs = new TaskCompletionSource<IQuerySnapshot>();

            //_query.GetDocuments((snapshot, error) =>
            //{
            //    if (error != null)
            //    {
            //        tcs.SetException(ExceptionMapper.Map(error));
            //    }
            //    else
            //    {
            //        tcs.SetResult(new QuerySnapshotWrapper(snapshot!));
            //    }
            //});

            //return tcs.Task;
        }

        public Task<IQuerySnapshot> GetDocumentsAsync(Source source)
        {
            return GetAsync(source);
        }

        public Task<IQuerySnapshot> GetAsync(Source source)
        {
            throw new NotImplementedException();
            //var tcs = new TaskCompletionSource<IQuerySnapshot>();

            //_query.GetDocuments(source.ToNative(), (snapshot, error) =>
            //{
            //    if (error != null)
            //    {
            //        tcs.SetException(ExceptionMapper.Map(error));
            //    }
            //    else
            //    {
            //        tcs.SetResult(new QuerySnapshotWrapper(snapshot!));
            //    }
            //});

            //return tcs.Task;
        }

        public IListenerRegistration AddSnapshotListener(QuerySnapshotHandler listener)
        {
            var registration = _query.Listen(snapshot =>
               {
                   listener?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot), null);
               });
            return new ListenerRegistrationWrapper(registration);
        }

        public IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, QuerySnapshotHandler listener)
        {
            throw new NotImplementedException();
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
