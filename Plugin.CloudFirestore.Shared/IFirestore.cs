using System;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public interface IFirestore
    {
        FirestoreSettings FirestoreSettings { get; set; }
        ICollectionReference GetCollection(string collectionPath);
        IDocumentReference GetDocument(string documentPath);
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
