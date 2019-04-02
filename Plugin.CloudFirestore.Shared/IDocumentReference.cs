using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public interface IDocumentReference
    {
        string Id { get; }
        string Path { get; }
        ICollectionReference Parent { get; }
        ICollectionReference GetCollection(string collectionPath);
        void GetDocument(DocumentSnapshotHandler handler);
        Task<IDocumentSnapshot> GetDocumentAsync();
        void SetData<T>(T documentData, CompletionHandler handler) where T : class;
        Task SetDataAsync<T>(T documentData) where T : class;
        void SetData<T>(T documentData, IEnumerable<string> mergeFields, CompletionHandler handler) where T : class;
        Task SetDataAsync<T>(T documentData, IEnumerable<string> mergeFields) where T : class;
        void SetData<T>(T documentData, bool merge, CompletionHandler handler) where T : class;
        Task SetDataAsync<T>(T documentData, bool merge) where T : class;
        void UpdateData<T>(T fields, CompletionHandler handler) where T : class;
        Task UpdateDataAsync<T>(T fields) where T : class;
        void UpdateData<T>(string field, T value, CompletionHandler handler, params object[] moreFieldsAndValues);
        Task UpdateDataAsync<T>(string field, T value, params object[] moreFieldsAndValues);
        void DeleteDocument(CompletionHandler handler);
        Task DeleteDocumentAsync();
        IListenerRegistration AddSnapshotListener(DocumentSnapshotHandler listener);
        IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, DocumentSnapshotHandler listener);
    }
}
