using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public interface IQuery
    {
        IQuery LimitTo(int limit);
        IQuery OrderBy(string field, bool descending);
        IQuery WhereEqualsTo<T>(string field, T value);
        IQuery WhereGreaterThan<T>(string field, T value);
        IQuery WhereGreaterThanOrEqualsTo<T>(string field, T value);
        IQuery WhereLessThan<T>(string field, T value);
        IQuery WhereLessThanOrEqualsTo<T>(string field, T value);
        IQuery StartAt(IDocumentSnapshot document);
        IQuery StartAt<T>(IEnumerable<T> fieldValues);
        IQuery StartAfter(IDocumentSnapshot document);
        IQuery StartAfter<T>(IEnumerable<T> fieldValues);
        IQuery EndAt(IDocumentSnapshot document);
        IQuery EndAt<T>(IEnumerable<T> fieldValues);
        IQuery EndBefore(IDocumentSnapshot document);
        IQuery EndBefore<T>(IEnumerable<T> fieldValues);
        void GetDocuments(QuerySnapshotHandler handler);
        Task<IQuerySnapshot> GetDocumentsAsync();
        IListenerRegistration AddSnapshotListener(QuerySnapshotHandler listener);
        IListenerRegistration AddSnapshotListener(bool includeMetadataChanges, QuerySnapshotHandler listener);
    }
}
