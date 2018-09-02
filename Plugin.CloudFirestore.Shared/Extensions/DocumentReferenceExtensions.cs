using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Plugin.CloudFirestore.Extensions
{
    public static class DocumentReferenceExtensions
    {
        public static IObservable<IDocumentSnapshot> AsObservable(this IDocumentReference document)
        {
            return Observable.Create<IDocumentSnapshot>(observer =>
            {
                var registration = document.AddSnapshotListener((snapshot, error) =>
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

        public static IObservable<IDocumentSnapshot> AsObservable(this IDocumentReference document, bool includeMetadataChanges)
        {
            return Observable.Create<IDocumentSnapshot>(observer =>
            {
                var registration = document.AddSnapshotListener(includeMetadataChanges, (snapshot, error) =>
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
    }
}
