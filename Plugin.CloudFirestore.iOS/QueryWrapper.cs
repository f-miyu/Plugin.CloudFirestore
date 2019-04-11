using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.CloudFirestore
{
    public class QueryWrapper : IQuery
    {
        private readonly Query _query;

        public QueryWrapper(Query query)
        {
            _query = query;
        }

        public IQuery LimitTo(int limit)
        {
            var query = _query.LimitedTo(limit);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field, bool descending)
        {
            var query = _query.OrderedBy(field, descending);
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo(string field, object value)
        {
            var query = _query.WhereEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThan(string field, object value)
        {
            var query = _query.WhereGreaterThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThanOrEqualsTo(string field, object value)
        {
            var query = _query.WhereGreaterThanOrEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThan(string field, object value)
        {
            var query = _query.WhereLessThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThanOrEqualsTo(string field, object value)
        {
            var query = _query.WhereLessThanOrEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery StartAt(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = _query.StartingAt((DocumentSnapshot)wrapper);
            return new QueryWrapper(query);
        }

        public IQuery StartAt(params object[] fieldValues)
        {
            var query = _query.StartingAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = _query.StartingAfter((DocumentSnapshot)wrapper);
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(params object[] fieldValues)
        {
            var query = _query.StartingAfter(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = _query.EndingAt((DocumentSnapshot)wrapper);
            return new QueryWrapper(query);
        }

        public IQuery EndAt(params object[] fieldValues)
        {
            var query = _query.EndingAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = _query.EndingBefore((DocumentSnapshot)wrapper);
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(params object[] fieldValues)
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

        public Task<IQuerySnapshot> GetDocumentsAsync()
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
                    tcs.SetResult(snapshot == null ? null : new QuerySnapshotWrapper(snapshot));
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
    }
}
