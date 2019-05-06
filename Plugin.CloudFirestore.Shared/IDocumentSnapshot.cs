using System;
using System.Collections.Generic;
namespace Plugin.CloudFirestore
{
    public interface IDocumentSnapshot
    {
        IDictionary<string, object> Data { get; }
        string Id { get; }
        bool Exists { get; }
        ISnapshotMetadata Metadata { get; }
        IDocumentReference Reference { get; }
        IDictionary<string, object> GetData(ServerTimestampBehavior serverTimestampBehavior);
        T ToObject<T>();
        T ToObject<T>(ServerTimestampBehavior serverTimestampBehavior);
    }
}
