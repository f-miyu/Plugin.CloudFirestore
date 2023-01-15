using System;
using System.Threading.Tasks;

namespace Plugin.CloudFirestore
{
    public class FirestoreWrapper : IFirestore, IEquatable<FirestoreWrapper>
    {
        private readonly Google.Cloud.Firestore.FirestoreDb _firestore;

        public FirestoreWrapper(Google.Cloud.Firestore.FirestoreDb firestore)
        {
            _firestore = firestore ?? throw new ArgumentNullException(nameof(firestore));
        }

        public IFirestoreSettings FirestoreSettings
        {
            get
            {
                throw new NotImplementedException();
                //new FirestoreSettings(_firestore.Settings);
            }
            set
            {
                throw new NotImplementedException();
                //return _firestore.Settings = value is not null ? new Google.Cloud.Firestore.V1.FirestoreSettings
                //{
                //    Host = value.Host,
                //    PersistenceEnabled = value.IsPersistenceEnabled,
                //    SslEnabled = value.IsSslEnabled,
                //    CacheSizeBytes = value.CacheSizeBytes
                //} : throw new ArgumentNullException();
            }
        }

        public ICollectionReference GetCollection(string collectionPath)
        {
            return Collection(collectionPath);
        }

        public ICollectionReference Collection(string collectionPath)
        {
            var collectionReference = _firestore.Collection(collectionPath);
            return new CollectionReferenceWrapper(collectionReference);
        }

        public IDocumentReference GetDocument(string documentPath)
        {
            return Document(documentPath);
        }

        public IDocumentReference Document(string documentPath)
        {
            var documentReference = _firestore.Document(documentPath);
            return new DocumentReferenceWrapper(documentReference);
        }

        public IQuery GetCollectionGroup(string collectionId)
        {
            return CollectionGroup(collectionId);
        }

        public IQuery CollectionGroup(string collectionId)
        {
            var query = _firestore.CollectionGroup(collectionId);
            return new QueryWrapper(query);
        }

        public void RunTransaction<T>(TransactionHandler<T> handler, CompletionHandler<T> completionHandler)
        {
            throw new NotImplementedException();
            //_firestore.RunTransactionAsync((transaction) =>
            //{
            //    var wrapper = new TransactionWrapper(transaction);
            //    handler(wrapper);
            //    return Task.CompletedTask;
            //}).ContinueWith(t =>
            //{
            //    completionHandler?.Invoke(handler., t.Exception);
            //});
        }

        public Task<T> RunTransactionAsync<T>(TransactionHandler<T> handler)
        {
            throw new NotImplementedException();
            //var tcs = new TaskCompletionSource<T>();
            //_firestore.RunTransactionAsync((transaction) =>
            //{
            //    var wrapper = new TransactionWrapper(transaction);
            //    return new ObjectHolder<T>(handler(wrapper));
            //}).ContinueWith(t =>
            //{
            //    if (t.IsFaulted)
            //    {
            //        tcs.SetException(t.Exception);
            //    }
            //    else if (t.IsCanceled)
            //    {
            //        tcs.SetCanceled();
            //    }
            //    else
            //    {
            //        tcs.SetResult(t.Result.Value);
            //    }
            //});

            //return tcs.Task;
        }

        public void RunTransaction(TransactionHandler handler, CompletionHandler completionHandler)
        {
            throw new NotImplementedException();
            //_firestore.RunTransaction((Transaction transaction, ref NSError error) =>
            //{
            //    try
            //    {
            //        var wrapper = new TransactionWrapper(transaction);
            //        handler(wrapper);
            //    }
            //    catch (NSErrorException e)
            //    {
            //        error = e.Error;
            //    }
            //    catch (Exception e)
            //    {
            //        error = new ExceptionError(e);
            //    }
            //    return null;
            //},
            //(NSObject? result, NSError? error) =>
            //{
            //    Exception? exception = null;

            //    if (error != null)
            //    {
            //        exception = error is ExceptionError exceptionError ? exceptionError.Exception : ExceptionMapper.Map(error);
            //    }

            //    completionHandler?.Invoke(exception);
            //});
        }

        public Task RunTransactionAsync(TransactionHandler handler)
        {
            throw new NotImplementedException();
            //var tcs = new TaskCompletionSource<bool>();

            //_firestore.RunTransaction((Transaction transaction, ref NSError error) =>
            //{
            //    try
            //    {
            //        var wrapper = new TransactionWrapper(transaction);
            //        handler(wrapper);
            //    }
            //    catch (NSErrorException e)
            //    {
            //        error = e.Error;
            //    }
            //    catch (Exception e)
            //    {
            //        error = new ExceptionError(e);
            //    }
            //    return null;
            //},
            //(NSObject? result, NSError? error) =>
            //{
            //    if (error != null)
            //    {
            //        if (error is ExceptionError exceptionError)
            //        {
            //            tcs.SetException(exceptionError.Exception);
            //        }
            //        else
            //        {
            //            tcs.SetException(ExceptionMapper.Map(error));
            //        }
            //    }
            //    else
            //    {
            //        tcs.SetResult(true);
            //    }
            //});

            //return tcs.Task;
        }

        public IWriteBatch CreateBatch()
        {
            return Batch();
        }

        public IWriteBatch Batch()
        {
            var writeBatch = _firestore.StartBatch();
            return new WriteBatchWrapper(writeBatch);
        }

        public void EnableNetwork(CompletionHandler handler)
        {
            throw new NotImplementedException();
            //_firestore.EnableNetwork((error) =>
            //{
            //    handler?.Invoke(error == null ? null : ExceptionMapper.Map(error));
            //});
        }

        public Task EnableNetworkAsync()
        {
            throw new NotImplementedException();
            //var tcs = new TaskCompletionSource<bool>();

            //_firestore.EnableNetwork((error) =>
            //{
            //    if (error != null)
            //    {
            //        tcs.SetException(ExceptionMapper.Map(error));
            //    }
            //    else
            //    {
            //        tcs.SetResult(true);
            //    }
            //});

            //return tcs.Task;
        }

        public void DisableNetwork(CompletionHandler handler)
        {
            throw new NotImplementedException();
            //_firestore.DisableNetwork((error) =>
            //{
            //    handler?.Invoke(error == null ? null : ExceptionMapper.Map(error));
            //});
        }

        public Task DisableNetworkAsync()
        {
            throw new NotImplementedException();
            //var tcs = new TaskCompletionSource<bool>();

            //_firestore.DisableNetwork((error) =>
            //{
            //    if (error != null)
            //    {
            //        tcs.SetException(ExceptionMapper.Map(error));
            //    }
            //    else
            //    {
            //        tcs.SetResult(true);
            //    }
            //});

            //return tcs.Task;
        }

        public IListenerRegistration AddSnapshotsInSyncListener(Action listener)
        {
            throw new NotImplementedException();
            //var registration = _firestore.AddSnapshotsInSyncListener(listener);
            //return new ListenerRegistrationWrapper(registration);
        }

        public Task ClearPersistenceAsync()
        {
            throw new NotImplementedException();

            //var tcs = new TaskCompletionSource<bool>();

            //_firestore.ClearPersistence((error) =>
            //{
            //    if (error != null)
            //    {
            //        tcs.SetException(ExceptionMapper.Map(error));
            //    }
            //    else
            //    {
            //        tcs.SetResult(true);
            //    }
            //});

            //return tcs.Task;
        }

        public Task TerminateAsync()
        {
            throw new NotImplementedException();
            //var tcs = new TaskCompletionSource<bool>();

            //_firestore.Terminate((error) =>
            //{
            //    if (error != null)
            //    {
            //        tcs.SetException(ExceptionMapper.Map(error));
            //    }
            //    else
            //    {
            //        tcs.SetResult(true);
            //    }
            //});

            //return tcs.Task;
        }

        public Task WaitForPendingWritesAsync()
        {
            throw new NotImplementedException();
            //var tcs = new TaskCompletionSource<bool>();

            //_firestore.WaitForPendingWrites((error) =>
            //{
            //    if (error != null)
            //    {
            //        tcs.SetException(ExceptionMapper.Map(error));
            //    }
            //    else
            //    {
            //        tcs.SetResult(true);
            //    }
            //});

            //return tcs.Task;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as FirestoreWrapper);
        }

        public bool Equals(FirestoreWrapper? other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_firestore, other._firestore)) return true;
            return _firestore.Equals(other._firestore);
        }

        public override int GetHashCode()
        {
            return _firestore?.GetHashCode() ?? 0;
        }
    }
}
