using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;

namespace Plugin.CloudFirestore
{
    public interface ICollectionReference : IQuery
    {
        string Id { get; }
        string Path { get; }
        IDocumentReference? Parent { get; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Document() method instead.")]
        IDocumentReference CreateDocument();
        IDocumentReference Document();
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Document(string documentPath) method instead.")]
        IDocumentReference GetDocument(string documentPath);
        IDocumentReference Document(string documentPath);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use AddAsync<T>(T data) method instead.")]
        void AddDocument(object data, CompletionHandler handler);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use AddAsync<T>(T data) method instead.")]
        Task AddDocumentAsync(object data);
        Task<IDocumentReference> AddAsync<T>(T data);
    }
}
