using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Plugin.CloudFirestore
{
    public partial interface IDocumentSnapshot
    {
        IDictionary<string, object?>? Data { get; }
        string Id { get; }
        bool Exists { get; }
        ISnapshotMetadata Metadata { get; }
        IDocumentReference Reference { get; }
        IDictionary<string, object?>? GetData(ServerTimestampBehavior serverTimestampBehavior);
        [return: MaybeNull] T ToObject<T>();
        [return: MaybeNull] T ToObject<T>(ServerTimestampBehavior serverTimestampBehavior);
        [return: MaybeNull] T Get<T>(string field);
        [return: MaybeNull] T Get<T>(string field, ServerTimestampBehavior serverTimestampBehavior);
        [return: MaybeNull] T Get<T>(FieldPath field);
        [return: MaybeNull] T Get<T>(FieldPath field, ServerTimestampBehavior serverTimestampBehavior);
    }
}
