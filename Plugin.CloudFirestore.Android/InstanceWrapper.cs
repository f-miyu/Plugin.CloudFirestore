using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using Android.Runtime;
using Firebase;

namespace Plugin.CloudFirestore
{
    public class InstanceWrapper : IInstance
    {
        public bool PersistenceEnabled
        {
            get => _instance.FirestoreSettings.IsPersistenceEnabled;
            set
            {
                var settings = new FirebaseFirestoreSettings.Builder()
                                                            .SetPersistenceEnabled(value)
                                                            .Build();

                _instance.FirestoreSettings = settings;
            }
        }

        private readonly FirebaseFirestore _instance;

        public InstanceWrapper(string appName = null)
        {
            if (!string.IsNullOrEmpty(appName))
            {
                var app = FirebaseApp.GetInstance(appName);
                _instance = FirebaseFirestore.GetInstance(app);
            }
            else if (!string.IsNullOrEmpty(CloudFirestore.DefaultAppName))
            {
                var app = FirebaseApp.GetInstance(CloudFirestore.DefaultAppName);
                _instance = FirebaseFirestore.GetInstance(app);
            }
            else
            {
                _instance = FirebaseFirestore.Instance;
            }
        }

        public ICollectionReference GetCollection(string collectionPath)
        {
            var collectionReference = _instance.Collection(collectionPath);
            return new CollectionReferenceWrapper(collectionReference);
        }

        public IDocumentReference GetDocument(string documentPath)
        {
            var documentReference = _instance.Document(documentPath);
            return new DocumentReferenceWrapper(documentReference);
        }

        public void RunTransaction<T>(TransactionHandler<T> handler, CompletionHandler<T> completionHandler)
        {
            _instance.RunTransaction(new UpdateFunction<T>(handler))
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
                             exception = task.Exception is ExceptionHolder wrappedException
                                 ? wrappedException.Exception
                                 : new CloudFirestoreException(task.Exception.Message);
                         }

                         completionHandler?.Invoke(result, exception);
                     }));
        }

        public Task<T> RunTransactionAsync<T>(TransactionHandler<T> handler)
        {
            var tcs = new TaskCompletionSource<T>();

            _instance.RunTransaction(new UpdateFunction<T>(handler))
                     .AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
                     {
                         if (task.IsSuccessful)
                         {
                             var result = task.Result.JavaCast<ObjectHolder<T>>();
                             tcs.SetResult(result.Object);
                         }
                         else
                         {
                             if (task.Exception is ExceptionHolder wrappedException)
                             {
                                 tcs.SetException(wrappedException.Exception);
                             }
                             else
                             {
                                 tcs.SetException(new CloudFirestoreException(task.Exception.Message));
                             }
                         }
                     }));

            return tcs.Task;
        }

        public void RunTransaction(TransactionHandler handler, CompletionHandler completionHandler)
        {
            _instance.RunTransaction(new UpdateFunction(handler))
                     .AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
                     {
                         Exception exception = null;

                         if (!task.IsSuccessful)
                         {
                             exception = task.Exception is ExceptionHolder wrappedException
                                 ? wrappedException.Exception
                                 : new CloudFirestoreException(task.Exception.Message);
                         }

                         completionHandler?.Invoke(exception);
                     }));
        }

        public Task RunTransactionAsync(TransactionHandler handler)
        {
            var tcs = new TaskCompletionSource<bool>();

            _instance.RunTransaction(new UpdateFunction(handler))
                     .AddOnCompleteListener(new OnCompleteHandlerListener((task) =>
                     {
                         if (task.IsSuccessful)
                         {
                             tcs.SetResult(true);
                         }
                         else
                         {
                             if (task.Exception is ExceptionHolder wrappedException)
                             {
                                 tcs.SetException(wrappedException.Exception);
                             }
                             else
                             {
                                 tcs.SetException(new CloudFirestoreException(task.Exception.Message));
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
                    throw new ExceptionHolder(e);
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
                    throw new ExceptionHolder(e);
                }
            }
        }

        public IWriteBatch CreateBatch()
        {
            var writeBatch = _instance.Batch();
            return new WriteBatchWrapper(writeBatch);
        }
    }
}
