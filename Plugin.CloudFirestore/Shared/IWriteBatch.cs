using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public interface IWriteBatch
    {
        [Obsolete("Please use CommitAsync() method instead.")]
        void Commit(CompletionHandler handler);
        Task CommitAsync();
        [Obsolete("Please use Set(IDocumentReference document, object documentData) method instead.")]
        void SetData(IDocumentReference document, object documentData);
        void Set(IDocumentReference document, object documentData);
        [Obsolete("Please use Set(IDocumentReference document, object documentData, params string[] mergeFields) method instead.")]
        void SetData(IDocumentReference document, object documentData, params string[] mergeFields);
        void Set(IDocumentReference document, object documentData, params string[] mergeFields);
        [Obsolete("Please use Set(IDocumentReference document, object documentData, params FieldPath[] mergeFields) method instead.")]
        void SetData(IDocumentReference document, object documentData, params FieldPath[] mergeFields);
        void Set(IDocumentReference document, object documentData, params FieldPath[] mergeFields);
        [Obsolete("Please use Set(IDocumentReference document, object documentData, bool merge) method instead.")]
        void SetData(IDocumentReference document, object documentData, bool merge);
        void Set(IDocumentReference document, object documentData, bool merge);
        [Obsolete("Please use Update(IDocumentReference document, object fields) method instead.")]
        void UpdateData(IDocumentReference document, object fields);
        void Update(IDocumentReference document, object fields);
        [Obsolete("Please use Update(IDocumentReference document, string field, object value, params object[] moreFieldsAndValues) method instead.")]
        void UpdateData(IDocumentReference document, string field, object value, params object[] moreFieldsAndValues);
        void Update(IDocumentReference document, string field, object value, params object[] moreFieldsAndValues);
        [Obsolete("Please use Update(IDocumentReference document, FieldPath field, object value, params object[] moreFieldsAndValues) method instead.")]
        void UpdateData(IDocumentReference document, FieldPath field, object value, params object[] moreFieldsAndValues);
        void Update(IDocumentReference document, FieldPath field, object value, params object[] moreFieldsAndValues);
        [Obsolete("Please use Delete(IDocumentReference document) method instead.")]
        void DeleteDocument(IDocumentReference document);
        void Delete(IDocumentReference document);
    }
}
