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
        private Query Query { get; }

        public QueryWrapper(Query query)
        {
            Query = query;
        }

        public IQuery LimitTo(int limit)
        {
            var query = Query.LimitedTo(limit);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field, bool descending)
        {
            var query = Query.OrderedBy(field, descending);
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo<T>(string field, T value)
        {
            var query = Query.WhereEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThan<T>(string field, T value)
        {
            var query = Query.WhereGreaterThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThanOrEqualsTo<T>(string field, T value)
        {
            var query = Query.WhereGreaterThanOrEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThan<T>(string field, T value)
        {
            var query = Query.WhereLessThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThanOrEqualsTo<T>(string field, T value)
        {
            var query = Query.WhereLessThanOrEqualsTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery StartAt(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = Query.StartingAt((DocumentSnapshot)wrapper);
            return new QueryWrapper(query);
        }

        public IQuery StartAt<T>(IEnumerable<T> fieldValues)
        {
            var query = Query.StartingAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = Query.StartingAfter((DocumentSnapshot)wrapper);
            return new QueryWrapper(query);
        }

        public IQuery StartAfter<T>(IEnumerable<T> fieldValues)
        {
            var query = Query.StartingAfter(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = Query.EndingAt((DocumentSnapshot)wrapper);
            return new QueryWrapper(query);
        }

        public IQuery EndAt<T>(IEnumerable<T> fieldValues)
        {
            var query = Query.EndingAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = Query.EndingBefore((DocumentSnapshot)wrapper);
            return new QueryWrapper(query);
        }

        public IQuery EndBefore<T>(IEnumerable<T> fieldValues)
        {
            var query = Query.EndingBefore(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public void GetDocuments(QuerySnapshotHandler handler)
        {
            Query.GetDocuments((snapshot, error) =>
            {
                handler?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });
        }

        public Task<IQuerySnapshot> GetDocumentsAsync()
        {
            var tcs = new TaskCompletionSource<IQuerySnapshot>();

            Query.GetDocuments((snapshot, error) =>
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

        public IListenerRegistration AddSnapshotListener(QuerySnapshotHandler listener)
        {
            var registration = Query.AddSnapshotListener((snapshot, error) =>
            {
                listener?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                 error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });

            return new ListenerRegistrationWrapper(registration);
        }

        public IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, QuerySnapshotHandler listener)
        {
            var registration = Query.AddSnapshotListener(includeMetadataChanges, (snapshot, error) =>
            {
                listener?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                 error == null ? null : new CloudFirestoreException(error.LocalizedDescription));
            });

            return new ListenerRegistrationWrapper(registration);
        }
    }
}
