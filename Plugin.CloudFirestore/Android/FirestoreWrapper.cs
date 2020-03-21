using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using Android.Runtime;
using Firebase;

namespace Plugin.CloudFirestore
{
    public class FirestoreWrapper : IFirestore, IEquatable<FirestoreWrapper>
    {
        public IFirestoreSettings FirestoreSettings
        {
            get => _firestore.FirestoreSettings == null ? null : new FirestoreSettings(_firestore.FirestoreSettings);
            set => _firestore.FirestoreSettings = value == null ? null : new FirebaseFirestoreSettings.Builder()
                .SetTimestampsInSnapshotsEnabled(value.AreTimestampsInSnapshotsEnabled)
                .SetHost(value.Host)
                .SetPersistenceEnabled(value.IsPersistenceEnabled)
                .SetSslEnabled(value.IsSslEnabled)
                .Build();
        }

        private readonly FirebaseFirestore _firestore;

        public FirestoreWrapper(FirebaseFirestore firestore)
        {
            _firestore = firestore;
        }

        public ICollectionReference GetCollection(string collectionPath)
        {
            var collectionReference = _firestore.Collection(collectionPath);
            return new CollectionReferenceWrapper(collectionReference);
        }

        public IDocumentReference GetDocument(string documentPath)
        {
            var documentReference = _firestore.Document(documentPath);
            return new DocumentReferenceWrapper(documentReference);
        }

        public IQuery GetCollectionGroup(string collectionId)
        {
            var query = _firestore.CollectionGroup(collectionId);
            return new QueryWrapper(query);
        }

        public void RunTransaction<T>(TransactionHandler<T> handler, CompletionHandler<T> completionHandler)
        {
            _firestore.RunTransaction(new UpdateFunction<T>(handler))
                     .AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
                     {
                         T result = default;
                         Exception exception = null;

                         if (task.IsSuccessful)
                         {
                             result = task.Result.JavaCast<ObjectHolder<T>>().Object;
                         }
                         else
                         {
                             exception = task.Exception is JavaException javaException
                                 ? javaException.Exception
                                 : ExceptionMapper.Map(task.Exception);
                         }

                         completionHandler?.Invoke(result, exception);
                     }));
        }

        public Task<T> RunTransactionAsync<T>(TransactionHandler<T> handler)
        {
            var tcs = new TaskCompletionSource<T>();

            _firestore.RunTransaction(new UpdateFunction<T>(handler))
                     .AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
                     {
                         if (task.IsSuccessful)
                         {
                             var result = task.Result.JavaCast<ObjectHolder<T>>();
                             tcs.SetResult(result.Object);
                         }
                         else
                         {
                             if (task.Exception is JavaException javaException)
                             {
                                 tcs.SetException(javaException.Exception);
                             }
                             else
                             {
                                 tcs.SetException(ExceptionMapper.Map(task.Exception));
                             }
                         }
                     }));

            return tcs.Task;
        }

        public void RunTransaction(TransactionHandler handler, CompletionHandler completionHandler)
        {
            _firestore.RunTransaction(new UpdateFunction(handler))
                     .AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
                     {
                         Exception exception = null;

                         if (!task.IsSuccessful)
                         {
                             exception = task.Exception is JavaException javaException
                                 ? javaException.Exception
                                 : ExceptionMapper.Map(task.Exception);
                         }

                         completionHandler?.Invoke(exception);
                     }));
        }

        public Task RunTransactionAsync(TransactionHandler handler)
        {
            var tcs = new TaskCompletionSource<bool>();

            _firestore.RunTransaction(new UpdateFunction(handler))
                     .AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
                     {
                         if (task.IsSuccessful)
                         {
                             tcs.SetResult(true);
                         }
                         else
                         {
                             if (task.Exception is JavaException nativeException)
                             {
                                 tcs.SetException(nativeException.Exception);
                             }
                             else
                             {
                                 tcs.SetException(ExceptionMapper.Map(task.Exception));
                             }
                         }
                     }));

            return tcs.Task;
        }

        private class UpdateFunction<T> : Java.Lang.Object, Transaction.IFunction
        {
            private readonly TransactionHandler<T> _handler;

            public UpdateFunction(TransactionHandler<T> handler)
            {
                _handler = handler;
            }

            public Java.Lang.Object Apply(Transaction transaction)
            {
                try
                {
                    var wrapper = new TransactionWrapper(transaction);
                    return new ObjectHolder<T>(_handler(wrapper));
                }
                catch (FirebaseFirestoreException)
                {
                    throw;
                }
                catch (System.Exception e)
                {
                    throw new JavaException(e);
                }
            }
        }

        private class UpdateFunction : Java.Lang.Object, Transaction.IFunction
        {
            private readonly TransactionHandler _handler;

            public UpdateFunction(TransactionHandler handler)
            {
                _handler = handler;
            }

            public Java.Lang.Object Apply(Transaction transaction)
            {
                try
                {
                    var wrapper = new TransactionWrapper(transaction);
                    _handler(wrapper);
                    return null;
                }
                catch (FirebaseFirestoreException)
                {
                    throw;
                }
                catch (System.Exception e)
                {
                    throw new JavaException(e);
                }
            }
        }

        public IWriteBatch CreateBatch()
        {
            var writeBatch = _firestore.Batch();
            return new WriteBatchWrapper(writeBatch);
        }

        public void EnableNetwork(CompletionHandler handler)
        {
            _firestore.EnableNetwork().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public Task EnableNetworkAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            _firestore.EnableNetwork().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                if (task.IsSuccessful)
                {
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            return tcs.Task;
        }

        public void DisableNetwork(CompletionHandler handler)
        {
            _firestore.DisableNetwork().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                handler?.Invoke(task.IsSuccessful ? null : ExceptionMapper.Map(task.Exception));
            }));
        }

        public Task DisableNetworkAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            _firestore.DisableNetwork().AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
            {
                if (task.IsSuccessful)
                {
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            return tcs.Task;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FirestoreWrapper);
        }

        public bool Equals(FirestoreWrapper other)
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
