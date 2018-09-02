using System;
namespace Plugin.CloudFirestore
{
    public interface IDocumentChange
    {
        IDocumentSnapshot Document { get; }
        int NewIndex { get; }
        int OldIndex { get; }
        DocumentChangeType Type { get; }
    }
}
