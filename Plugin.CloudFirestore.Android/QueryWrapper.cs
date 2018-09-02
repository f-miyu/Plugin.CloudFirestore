using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Android.Runtime;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public class QueryWrapper : IQuery
    {
        public Query Query { get; }

        public QueryWrapper(Query query)
        {
            Query = query;
        }

        public IQuery LimitTo(int limit)
        {
            var query = Query.Limit(limit);
            return new QueryWrapper(query);
        }

        public IQuery OrderBy(string field, bool descending)
        {
            var direction = descending ? Query.Direction.Descending : Query.Direction.Ascending;

            var query = Query.OrderBy(field, direction);
            return new QueryWrapper(query);
        }

        public IQuery WhereEqualsTo<T>(string field, T value)
        {
            var query = Query.WhereEqualTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThan<T>(string field, T value)
        {
            var query = Query.WhereGreaterThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereGreaterThanOrEqualsTo<T>(string field, T value)
        {
            var query = Query.WhereGreaterThanOrEqualTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThan<T>(string field, T value)
        {
            var query = Query.WhereLessThan(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery WhereLessThanOrEqualsTo<T>(string field, T value)
        {
            var query = Query.WhereLessThanOrEqualTo(field, value.ToNativeFieldValue());
            return new QueryWrapper(query);
        }

        public IQuery StartAt(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = Query.StartAt(wrapper.DocumentSnapshot);
            return new QueryWrapper(query);
        }

        public IQuery StartAt<T>(IEnumerable<T> fieldValues)
        {
            var query = Query.StartAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery StartAfter(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = Query.StartAfter(wrapper.DocumentSnapshot);
            return new QueryWrapper(query);
        }

        public IQuery StartAfter<T>(IEnumerable<T> fieldValues)
        {
            var query = Query.StartAfter(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndAt(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = Query.EndAt(wrapper.DocumentSnapshot);
            return new QueryWrapper(query);
        }

        public IQuery EndAt<T>(IEnumerable<T> fieldValues)
        {
            var query = Query.EndAt(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public IQuery EndBefore(IDocumentSnapshot document)
        {
            var wrapper = (DocumentSnapshotWrapper)document;
            var query = Query.EndBefore(wrapper.DocumentSnapshot);
            return new QueryWrapper(query);
        }

        public IQuery EndBefore<T>(IEnumerable<T> fieldValues)
        {
            var query = Query.EndBefore(fieldValues.Select(x => x.ToNativeFieldValue()).ToArray());
            return new QueryWrapper(query);
        }

        public void GetDocuments(QuerySnapshotHandler handler)
        {
            Query.Get().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                var snapshot = !task.IsSuccessful ? null : task.Result.JavaCast<QuerySnapshot>();
                handler?.Invoke(snapshot == null ? null : new QuerySnapshotWrapper(snapshot),
                                task.IsSuccessful ? null : new CloudFirestoreException(task.Exception.Message));
            }));
        }

        public Task<IQuerySnapshot> GetDocumentsAsync()
        {
            var tcs = new TaskCompletionSource<IQuerySnapshot>();

            Query.Get().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                if (task.IsSuccessful)
                {
                    var snapshot = task.Result.JavaCast<QuerySnapshot>();
                    tcs.SetResult(snapshot == null ? null : new QuerySnapshotWrapper(snapshot));
                }
                else
                {
                    tcs.SetException(new CloudFirestoreException(task.Exception.Message));
                }
            }));

            return tcs.Task;
        }

        public IListenerRegistration AddSnapshotListener(QuerySnapshotHandler listener)
        {
            var registration = Query.AddSnapshotListener(new EventHandlerListener<QuerySnapshot>((value, error) =>
            {
                listener?.Invoke(value == null ? null : new QuerySnapshotWrapper(value),
                                 error == null ? null : new CloudFirestoreException(error.Message));
            }));

            return new ListenerRegistrationWrapper(registration);
        }

        public IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, QuerySnapshotHandler listener)
        {
            if (!includeMetadataChanges)
            {
                return AddSnapshotListener(listener);
            }

            var option = new QueryListenOptions().IncludeQueryMetadataChanges();

            var registration = Query.AddSnapshotListener(option, new EventHandlerListener<QuerySnapshot>((value, error) =>
            {
                listener?.Invoke(value == null ? null : new QuerySnapshotWrapper(value),
                                 error == null ? null : new CloudFirestoreException(error.Message));
            }));

            return new ListenerRegistrationWrapper(registration);
        }
    }
}
