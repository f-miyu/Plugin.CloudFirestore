using System;
using System.Diagnostics.CodeAnalysis;

namespace Plugin.CloudFirestore
{
    public delegate void DocumentSnapshotHandler(IDocumentSnapshot? snapshot, Exception? error);
    public delegate void QuerySnapshotHandler(IQuerySnapshot? snapshot, Exception? error);
    public delegate void CompletionHandler(Exception? error);
    public delegate void CompletionHandler<T>([AllowNull] T result, Exception? error);
    public delegate void TransactionHandler(ITransaction transaction);
    public delegate T TransactionHandler<T>(ITransaction transaction);
}
