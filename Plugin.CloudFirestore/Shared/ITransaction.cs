using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public interface ITransaction
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Get() method instead.")]
        IDocumentSnapshot GetDocument(IDocumentReference document);
        IDocumentSnapshot Get(IDocumentReference document);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Set<T>(IDocumentReference document, T documentData) method instead.")]
        void SetData(IDocumentReference document, object documentData);
        ITransaction Set<T>(IDocumentReference document, T documentData);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Set<T>(IDocumentReference document, T documentData, params string[] mergeFields) method instead.")]
        void SetData(IDocumentReference document, object documentData, params string[] mergeFields);
        ITransaction Set<T>(IDocumentReference document, T documentData, params string[] mergeFields);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Set<T>(IDocumentReference document, T documentData, params FieldPath[] mergeFields) method instead.")]
        void SetData(IDocumentReference document, object documentData, params FieldPath[] mergeFields);
        ITransaction Set<T>(IDocumentReference document, T documentData, params FieldPath[] mergeFields);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Set<T>(IDocumentReference document, T documentData, bool merge) method instead.")]
        void SetData(IDocumentReference document, object documentData, bool merge);
        ITransaction Set<T>(IDocumentReference document, T documentData, bool merge);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Update<T>(IDocumentReference document, T fields) method instead.")]
        void UpdateData(IDocumentReference document, object fields);
        ITransaction Update<T>(IDocumentReference document, T fields);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Update(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues) method instead.")]
        void UpdateData(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues);
        ITransaction Update(IDocumentReference document, string field, object? value, params object?[] moreFieldsAndValues);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Update(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues) method instead.")]
        void UpdateData(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues);
        ITransaction Update(IDocumentReference document, FieldPath field, object? value, params object?[] moreFieldsAndValues);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Delete(IDocumentReference document) method instead.")]
        void DeleteDocument(IDocumentReference document);
        ITransaction Delete(IDocumentReference document);
    }
}
