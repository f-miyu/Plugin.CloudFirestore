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
        void SetData(object documentData, CompletionHandler handler);
        Task SetDataAsync(object documentData);
        void SetData(object documentData, CompletionHandler handler, params string[] mergeFields);
        Task SetDataAsync(object documentData, params string[] mergeFields);
        void SetData(object documentData, bool merge, CompletionHandler handler);
        Task SetDataAsync(object documentData, bool merge);
        void UpdateData(object fields, CompletionHandler handler);
        Task UpdateDataAsync(object fields);
        void UpdateData(string field, object value, CompletionHandler handler, params object[] moreFieldsAndValues);
        Task UpdateDataAsync(string field, object value, params object[] moreFieldsAndValues);
        void DeleteDocument(CompletionHandler handler);
        Task DeleteDocumentAsync();
        IListenerRegistration AddSnapshotListener(DocumentSnapshotHandler listener);
        IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, DocumentSnapshotHandler listener);
    }
}
