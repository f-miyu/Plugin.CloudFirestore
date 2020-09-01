using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public partial interface IDocumentReference
    {
        string Id { get; }
        string Path { get; }
        ICollectionReference Parent { get; }
        IFirestore Firestore { get; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Collection(string collectionPath) method instead.")]
        ICollectionReference GetCollection(string collectionPath);
        ICollectionReference Collection(string collectionPath);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use GetAsync() method instead.")]
        void GetDocument(DocumentSnapshotHandler handler);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use GetAsync(Source source) method instead.")]
        void GetDocument(Source source, DocumentSnapshotHandler handler);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use GetAsync() method instead.")]
        Task<IDocumentSnapshot> GetDocumentAsync();
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use GetAsync(Source source) method instead.")]
        Task<IDocumentSnapshot> GetDocumentAsync(Source source);
        Task<IDocumentSnapshot> GetAsync();
        Task<IDocumentSnapshot> GetAsync(Source source);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use SetAsync<T>(T documentData) method instead.")]
        void SetData(object documentData, CompletionHandler handler);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use SetAsync<T>(T documentData) method instead.")]
        Task SetDataAsync(object documentData);
        Task SetAsync<T>(T documentData);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use SetAsync<T>(T documentData, params string[] mergeFields) method instead.")]
        void SetData(object documentData, CompletionHandler handler, params string[] mergeFields);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use SetAsync<T>(T documentData, params FieldPath[] mergeFields) method instead.")]
        void SetData(object documentData, CompletionHandler handler, params FieldPath[] mergeFields);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use SetAsync<T>(T documentData, params string[] mergeFields) method instead.")]
        Task SetDataAsync(object documentData, params string[] mergeFields);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use SetAsync<T>(T documentData, params FieldPath[] mergeFields) method instead.")]
        Task SetDataAsync(object documentData, params FieldPath[] mergeFields);
        Task SetAsync<T>(T documentData, params string[] mergeFields);
        Task SetAsync<T>(T documentData, params FieldPath[] mergeFields);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use SetAsync<T>(T documentData, bool merge) method instead.")]
        void SetData(object documentData, bool merge, CompletionHandler handler);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use SetAsync<T>(T documentData, bool merge) method instead.")]
        Task SetDataAsync(object documentData, bool merge);
        Task SetAsync<T>(T documentData, bool merge);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use UpdateAsync<T>(T fields) method instead.")]
        void UpdateData(object fields, CompletionHandler handler);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use UpdateAsync<T>(T fields) method instead.")]
        Task UpdateDataAsync(object fields);
        Task UpdateAsync<T>(T fields);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use UpdateAsync(string field, object? value, params object?[] moreFieldsAndValues) method instead.")]
        void UpdateData(string field, object? value, CompletionHandler handler, params object?[] moreFieldsAndValues);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use UpdateAsync(FieldPath field, object? value, params object?[] moreFieldsAndValues) method instead.")]
        void UpdateData(FieldPath field, object? value, CompletionHandler handler, params object?[] moreFieldsAndValues);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use UpdateAsync(string field, object? value, params object?[] moreFieldsAndValues) method instead.")]
        Task UpdateDataAsync(string field, object? value, params object?[] moreFieldsAndValues);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use UpdateAsync(FieldPath field, object? value, params object?[] moreFieldsAndValues) method instead.")]
        Task UpdateDataAsync(FieldPath field, object? value, params object?[] moreFieldsAndValues);
        Task UpdateAsync(string field, object? value, params object?[] moreFieldsAndValues);
        Task UpdateAsync(FieldPath field, object? value, params object?[] moreFieldsAndValues);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use DeleteAsync() method instead.")]
        void DeleteDocument(CompletionHandler handler);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use DeleteAsync() method instead.")]
        Task DeleteDocumentAsync();
        Task DeleteAsync();
        IListenerRegistration AddSnapshotListener(DocumentSnapshotHandler listener);
        IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, DocumentSnapshotHandler listener);
    }
}
