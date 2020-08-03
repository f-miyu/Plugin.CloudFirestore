using System;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace Plugin.CloudFirestore
{
    public interface ICollectionReference : IQuery
    {
        string Id { get; }
        string Path { get; }
        IDocumentReference Parent { get; }
        [Obsolete("Please use Document() method instead.")]
        IDocumentReference CreateDocument();
        IDocumentReference Document();
        [Obsolete("Please use Document(string documentPath) method instead.")]
        IDocumentReference GetDocument(string documentPath);
        IDocumentReference Document(string documentPath);
        [Obsolete("Please use AddAsync<T>(T data) method instead.")]
        void AddDocument(object data, CompletionHandler handler);
        [Obsolete("Please use AddAsync<T>(T data) method instead.")]
        Task AddDocumentAsync(object data);
        Task AddAsync<T>(T data);
    }
}
