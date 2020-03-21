using System;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public interface IFirestore
    {
        IFirestoreSettings FirestoreSettings { get; set; }
        ICollectionReference GetCollection(string collectionPath);
        IDocumentReference GetDocument(string documentPath);
        IQuery GetCollectionGroup(string collectionId);
        void RunTransaction<T>(TransactionHandler<T> handler, CompletionHandler<T> completionHandler);
        Task<T> RunTransactionAsync<T>(TransactionHandler<T> handler);
        void RunTransaction(TransactionHandler handler, CompletionHandler completionHandler);
        Task RunTransactionAsync(TransactionHandler handler);
        IWriteBatch CreateBatch();
        void EnableNetwork(CompletionHandler handler);
        Task EnableNetworkAsync();
        void DisableNetwork(CompletionHandler handler);
        Task DisableNetworkAsync();
    }
}
