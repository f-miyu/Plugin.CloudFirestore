using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public interface IQuery
    {
        IFirestore Firestore { get; }
        IQuery LimitTo(long limit);
        IQuery OrderBy(string field);
        IQuery OrderBy(FieldPath field);
        IQuery OrderBy(string field, bool descending);
        IQuery OrderBy(FieldPath field, bool descending);
        IQuery WhereEqualsTo(string field, object value);
        IQuery WhereEqualsTo(FieldPath field, object value);
        IQuery WhereGreaterThan(string field, object value);
        IQuery WhereGreaterThan(FieldPath field, object value);
        IQuery WhereGreaterThanOrEqualsTo(string field, object value);
        IQuery WhereGreaterThanOrEqualsTo(FieldPath field, object value);
        IQuery WhereLessThan(string field, object value);
        IQuery WhereLessThan(FieldPath field, object value);
        IQuery WhereLessThanOrEqualsTo(string field, object value);
        IQuery WhereLessThanOrEqualsTo(FieldPath field, object value);
        IQuery WhereArrayContains(string field, object value);
        IQuery WhereArrayContains(FieldPath field, object value);
        IQuery StartAt(IDocumentSnapshot document);
        IQuery StartAt(params object[] fieldValues);
        IQuery StartAfter(IDocumentSnapshot document);
        IQuery StartAfter(params object[] fieldValues);
        IQuery EndAt(IDocumentSnapshot document);
        IQuery EndAt(params object[] fieldValues);
        IQuery EndBefore(IDocumentSnapshot document);
        IQuery EndBefore(params object[] fieldValues);
        void GetDocuments(QuerySnapshotHandler handler);
        void GetDocuments(Source source, QuerySnapshotHandler handler);
        Task<IQuerySnapshot> GetDocumentsAsync();
        Task<IQuerySnapshot> GetDocumentsAsync(Source source);
        IListenerRegistration AddSnapshotListener(QuerySnapshotHandler listener);
        IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, QuerySnapshotHandler listener);
    }
}
