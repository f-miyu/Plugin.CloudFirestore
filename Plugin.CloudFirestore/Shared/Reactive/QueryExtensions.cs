using System;
using System.Linq;
using System.Reactive.Linq;

namespace Plugin.CloudFirestore.Reactive
{
    public static class QueryExtensions
    {
        public static IObservable<IQuerySnapshot> AsObservable(this IQuery query)
        {
            return Observable.Create<IQuerySnapshot>(observer =>
            {
                return query.AddSnapshotListener((snapshot, error) =>
                {
                    if (error != null)
                    {
                        observer.OnError(error);
                    }
                    else
                    {
                        observer.OnNext(snapshot!);
                    }
                });
            });
        }

        public static IObservable<IQuerySnapshot> AsObservable(this IQuery query, bool includeMetadataChanges)
        {
            return Observable.Create<IQuerySnapshot>(observer =>
            {
                return query.AddSnapshotListener(includeMetadataChanges, (snapshot, error) =>
                {
                    if (error != null)
                    {
                        observer.OnError(error);
                    }
                    else
                    {
                        observer.OnNext(snapshot!);
                    }
                });
            });
        }

        public static IObservable<IDocumentChange> ObserveAdded(this IQuery query)
        {
            return query.AsObservable()
                        .Where(x => x != null)
                        .SelectMany(x => x.DocumentChanges.Where(y => y.Type == DocumentChangeType.Added));
        }

        public static IObservable<IDocumentChange> ObserveAdded(this IQuery query, bool includeMetadataChanges)
        {
            return query.AsObservable(includeMetadataChanges)
                        .Where(x => x != null)
                        .SelectMany(x => x.DocumentChanges.Where(y => y.Type == DocumentChangeType.Added));
        }

        public static IObservable<IDocumentChange> ObserveModified(this IQuery query)
        {
            return query.AsObservable()
                        .Where(x => x != null)
                        .SelectMany(x => x.DocumentChanges.Where(y => y.Type == DocumentChangeType.Modified));
        }

        public static IObservable<IDocumentChange> ObserveModified(this IQuery query, bool includeMetadataChanges)
        {
            return query.AsObservable(includeMetadataChanges)
                        .Where(x => x != null)
                        .SelectMany(x => x.DocumentChanges.Where(y => y.Type == DocumentChangeType.Modified));
        }

        public static IObservable<IDocumentChange> ObserveRemoved(this IQuery query)
        {
            return query.AsObservable()
                        .Where(x => x != null)
                        .SelectMany(x => x.DocumentChanges.Where(y => y.Type == DocumentChangeType.Removed));
        }

        public static IObservable<IDocumentChange> ObserveRemoved(this IQuery query, bool includeMetadataChanges)
        {
            return query.AsObservable(includeMetadataChanges)
                        .Where(x => x != null)
                        .SelectMany(x => x.DocumentChanges.Where(y => y.Type == DocumentChangeType.Removed));
        }
    }
}
