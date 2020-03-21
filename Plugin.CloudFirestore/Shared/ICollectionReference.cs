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
        IDocumentReference CreateDocument();
        IDocumentReference GetDocument(string documentPath);
        void AddDocument(object data, CompletionHandler handler);
        Task AddDocumentAsync(object data);
    }
}
