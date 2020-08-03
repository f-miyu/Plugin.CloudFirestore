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
        IFirestore Firestore { get; }
        [Obsolete("Please use Collection(string collectionPath) method instead.")]
        ICollectionReference GetCollection(string collectionPath);
        ICollectionReference Collection(string collectionPath);
        [Obsolete("Please use GetAsync() method instead.")]
        void GetDocument(DocumentSnapshotHandler handler);
        [Obsolete("Please use GetAsync(Source source) method instead.")]
        void GetDocument(Source source, DocumentSnapshotHandler handler);
        [Obsolete("Please use GetAsync() method instead.")]
        Task<IDocumentSnapshot> GetDocumentAsync();
        [Obsolete("Please use GetAsync(Source source) method instead.")]
        Task<IDocumentSnapshot> GetDocumentAsync(Source source);
        Task<IDocumentSnapshot> GetAsync();
        Task<IDocumentSnapshot> GetAsync(Source source);
        [Obsolete("Please use SetAsync<T>(T documentData) method instead.")]
        void SetData(object documentData, CompletionHandler handler);
        [Obsolete("Please use SetAsync<T>(T documentData) method instead.")]
        Task SetDataAsync(object documentData);
        Task SetAsync<T>(T documentData);
        [Obsolete("Please use SetAsync<T>(T documentData, params string[] mergeFields) method instead.")]
        void SetData(object documentData, CompletionHandler handler, params string[] mergeFields);
        [Obsolete("Please use SetAsync<T>(T documentData, params FieldPath[] mergeFields) method instead.")]
        void SetData(object documentData, CompletionHandler handler, params FieldPath[] mergeFields);
        [Obsolete("Please use SetAsync<T>(T documentData, params string[] mergeFields) method instead.")]
        Task SetDataAsync(object documentData, params string[] mergeFields);
        [Obsolete("Please use SetAsync<T>(T documentData, params FieldPath[] mergeFields) method instead.")]
        Task SetDataAsync(object documentData, params FieldPath[] mergeFields);
        Task SetAsync<T>(T documentData, params string[] mergeFields);
        Task SetAsync<T>(T documentData, params FieldPath[] mergeFields);
        [Obsolete("Please use SetAsync<T>(T documentData, bool merge) method instead.")]
        void SetData(object documentData, bool merge, CompletionHandler handler);
        [Obsolete("Please use SetAsync<T>(T documentData, bool merge) method instead.")]
        Task SetDataAsync(object documentData, bool merge);
        Task SetAsync<T>(T documentData, bool merge);
        [Obsolete("Please use UpdateAsync<T>(T fields) method instead.")]
        void UpdateData(object fields, CompletionHandler handler);
        [Obsolete("Please use UpdateAsync<T>(T fields) method instead.")]
        Task UpdateDataAsync(object fields);
        Task UpdateAsync<T>(T fields);
        [Obsolete("Please use UpdateAsync(string field, object value, params object[] moreFieldsAndValues) method instead.")]
        void UpdateData(string field, object value, CompletionHandler handler, params object[] moreFieldsAndValues);
        [Obsolete("Please use UpdateAsync(FieldPath field, object value, params object[] moreFieldsAndValues) method instead.")]
        void UpdateData(FieldPath field, object value, CompletionHandler handler, params object[] moreFieldsAndValues);
        [Obsolete("Please use UpdateAsync(string field, object value, params object[] moreFieldsAndValues) method instead.")]
        Task UpdateDataAsync(string field, object value, params object[] moreFieldsAndValues);
        [Obsolete("Please use UpdateAsync(FieldPath field, object value, params object[] moreFieldsAndValues) method instead.")]
        Task UpdateDataAsync(FieldPath field, object value, params object[] moreFieldsAndValues);
        Task UpdateAsync(string field, object value, params object[] moreFieldsAndValues);
        Task UpdateAsync(FieldPath field, object value, params object[] moreFieldsAndValues);
        [Obsolete("Please use DeleteAsync() method instead.")]
        void DeleteDocument(CompletionHandler handler);
        [Obsolete("Please use DeleteAsync() method instead.")]
        Task DeleteDocumentAsync();
        Task DeleteAsync();
        IListenerRegistration AddSnapshotListener(DocumentSnapshotHandler listener);
        IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, DocumentSnapshotHandler listener);
    }
}
