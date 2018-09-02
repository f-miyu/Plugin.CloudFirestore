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
        T ToObject<T>() where T : class;
    }
}
