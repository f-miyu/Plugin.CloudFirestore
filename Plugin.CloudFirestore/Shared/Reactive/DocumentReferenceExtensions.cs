using System;
using System.Reactive.Linq;

namespace Plugin.CloudFirestore.Reactive
{
    public static class DocumentReferenceExtensions
    {
        public static IObservable<IDocumentSnapshot> AsObservable(this IDocumentReference document)
        {
            return Observable.Create<IDocumentSnapshot>(observer =>
            {
                return document.AddSnapshotListener((snapshot, error) =>
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

        public static IObservable<IDocumentSnapshot> AsObservable(this IDocumentReference document, bool includeMetadataChanges)
        {
            return Observable.Create<IDocumentSnapshot>(observer =>
            {
                return document.AddSnapshotListener(includeMetadataChanges, (snapshot, error) =>
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
    }
}
