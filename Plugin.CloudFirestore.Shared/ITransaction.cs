using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public interface ITransaction
    {
        IDocumentSnapshot GetDocument(IDocumentReference document);
        void SetData<T>(IDocumentReference document, T documentData) where T : class;
        void SetData<T>(IDocumentReference document, T documentData, string[] mergeFields) where T : class;
        void SetData<T>(IDocumentReference document, T documentData, bool merge) where T : class;
        void UpdateData<T>(IDocumentReference document, T fields) where T : class;
        void UpdateData<T>(IDocumentReference document, string field, T value, params object[] moreFieldsAndValues);
        void DeleteDocument(IDocumentReference document);
    }
}
