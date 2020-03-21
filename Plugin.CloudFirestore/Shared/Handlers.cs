using System;

namespace Plugin.CloudFirestore
{
    public delegate void DocumentSnapshotHandler(IDocumentSnapshot snapshot, Exception error);
    public delegate void QuerySnapshotHandler(IQuerySnapshot snapshot, Exception error);
    public delegate void CompletionHandler(Exception error);
    public delegate void CompletionHandler<T>(T result, Exception error);
    public delegate void TransactionHandler(ITransaction transaction);
    public delegate T TransactionHandler<T>(ITransaction transaction);
}
