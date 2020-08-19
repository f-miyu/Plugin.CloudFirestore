using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public interface IFirestore
    {
        IFirestoreSettings FirestoreSettings { get; set; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use GetCollection(string collectionPath) method instead.")]
        ICollectionReference GetCollection(string collectionPath);
        ICollectionReference Collection(string collectionPath);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Document(string documentPath) method instead.")]
        IDocumentReference GetDocument(string documentPath);
        IDocumentReference Document(string documentPath);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use CollectionGroup(string collectionId) method instead.")]
        IQuery GetCollectionGroup(string collectionId);
        IQuery CollectionGroup(string collectionId);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use RunTransactionAsync<T>(TransactionHandler<T> handler) method instead.")]
        void RunTransaction<T>(TransactionHandler<T> handler, CompletionHandler<T> completionHandler);
        Task<T> RunTransactionAsync<T>(TransactionHandler<T> handler);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use RunTransactionAsync(TransactionHandler handler) method instead.")]
        void RunTransaction(TransactionHandler handler, CompletionHandler completionHandler);
        Task RunTransactionAsync(TransactionHandler handler);
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Batch() method instead.")]
        IWriteBatch CreateBatch();
        IWriteBatch Batch();
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use EnableNetworkAsync() method instead.")]
        void EnableNetwork(CompletionHandler handler);
        Task EnableNetworkAsync();
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use DisableNetworkAsync() method instead.")]
        void DisableNetwork(CompletionHandler handler);
        Task DisableNetworkAsync();
        IListenerRegistration AddSnapshotsInSyncListener(Action listener);
        Task ClearPersistenceAsync();
        Task TerminateAsync();
        Task WaitForPendingWritesAsync();
    }
}
