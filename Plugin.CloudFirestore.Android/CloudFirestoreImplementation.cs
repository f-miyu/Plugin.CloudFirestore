using System;
using System.Threading.Tasks;
using Firebase.Firestore;
using Android.Runtime;

namespace Plugin.CloudFirestore
{
    public class CloudFirestoreImplementation : ICloudFirestore
    {
        public ICollectionReference GetCollection(string collectionPath)
        {
            var collectionReference = CloudFirestore.Instance.Collection(collectionPath);
            return new CollectionReferenceWrapper(collectionReference);
        }

        public IDocumentReference GetDocument(string documentPath)
        {
            var documentReference = CloudFirestore.Instance.Document(documentPath);
            return new DocumentReferenceWrapper(documentReference);
        }

        public void RunTransaction<T>(TransactionHandler<T> handler, CompletionHandler<T> completionHandler)
        {
            CloudFirestore.Instance.RunTransaction(new UpdateFunction<T>(handler))
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

            CloudFirestore.Instance.RunTransaction(new UpdateFunction<T>(handler))
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
            CloudFirestore.Instance.RunTransaction(new UpdateFunction(handler))
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

            CloudFirestore.Instance.RunTransaction(new UpdateFunction(handler))
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
            var writeBatch = CloudFirestore.Instance.Batch();
            return new WriteBatchWrapper(writeBatch);
        }
    }
}
