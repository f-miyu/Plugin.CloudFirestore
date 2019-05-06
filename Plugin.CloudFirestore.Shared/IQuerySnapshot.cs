using System;
using System.Collections.Generic;
namespace Plugin.CloudFirestore
{
    public interface IQuerySnapshot
    {
        int Count { get; }
        IEnumerable<IDocumentChange> DocumentChanges { get; }
        IEnumerable<IDocumentSnapshot> Documents { get; }
        bool IsEmpty { get; }
        ISnapshotMetadata Metadata { get; }
        IQuery Query { get; }
        IEnumerable<T> ToObjects<T>();
        IEnumerable<T> ToObjects<T>(ServerTimestampBehavior serverTimestampBehavior);
    }
}
