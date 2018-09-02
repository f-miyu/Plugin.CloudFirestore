using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Plugin.CloudFirestore.Extensions
{
    public static class QueryExtensions
    {
        public static IObservable<IQuerySnapshot> AsObservable(this IQuery query)
        {
            return Observable.Create<IQuerySnapshot>(observer =>
            {
                var registration = query.AddSnapshotListener((snapshot, error) =>
                {
                    if (error != null)
                    {
                        observer.OnError(error);
                    }
                    else
                    {
                        observer.OnNext(snapshot);
                    }
                });

                return Disposable.Create(registration.Remove);
            });
        }

        public static IObservable<IQuerySnapshot> AsObservable(this IQuery query, bool includeMetadataChanges)
        {
            return Observable.Create<IQuerySnapshot>(observer =>
            {
                var registration = query.AddSnapshotListener(includeMetadataChanges, (snapshot, error) =>
                {
                    if (error != null)
                    {
                        observer.OnError(error);
                    }
                    else
                    {
                        observer.OnNext(snapshot);
                    }
                });

                return Disposable.Create(registration.Remove);
            });
        }

        public static IObservable<IDocumentSnapshot> ObserveAdded(this IQuery query)
        {
            return query.AsObservable()
                        .Where(x => x != null)
                        .SelectMany(x => x.DocumentChanges.Where(y => y.Type == DocumentChangeType.Added).Select(y => y.Document));
        }

        public static IObservable<IDocumentSnapshot> ObserveModified(this IQuery query)
        {
            return query.AsObservable()
                        .Where(x => x != null)
                        .SelectMany(x => x.DocumentChanges.Where(y => y.Type == DocumentChangeType.Modified).Select(y => y.Document));
        }

        public static IObservable<IDocumentSnapshot> ObserveRemoved(this IQuery query)
        {
            return query.AsObservable()
                        .Where(x => x != null)
                        .SelectMany(x => x.DocumentChanges.Where(y => y.Type == DocumentChangeType.Removed).Select(y => y.Document));
        }
    }
}
