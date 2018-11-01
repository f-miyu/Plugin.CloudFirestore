using System;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using Foundation;

namespace Plugin.CloudFirestore
{
    public class InstanceWrapper : IInstance
    {
        public bool PersistenceEnabled
        {
            get => _instance.Settings.PersistenceEnabled;
            set
            {
                var settings = new FirestoreSettings();
                settings.PersistenceEnabled = value;
                _instance.Settings = settings;
            }
        }

        private readonly Firestore _instance;

        public InstanceWrapper(string appName = null)
        {
            if (!string.IsNullOrEmpty(appName))
            {
                var app = Firebase.Core.App.From(appName);
                _instance = Firestore.Create(app);
            }
            else if (!string.IsNullOrEmpty(CloudFirestore.DefaultAppName))
            {
                var app = Firebase.Core.App.From(CloudFirestore.DefaultAppName);
                _instance = Firestore.Create(app);
            }
            else
            {
                _instance = Firestore.SharedInstance;
            }
        }

        public ICollectionReference GetCollection(string collectionPath)
        {
            var collectionReference = _instance.GetCollection(collectionPath);
            return new CollectionReferenceWrapper(collectionReference);
        }

        public IDocumentReference GetDocument(string documentPath)
        {
            var documentReference = _instance.GetDocument(documentPath);
            return new DocumentReferenceWrapper(documentReference);
        }

        public void RunTransaction<T>(TransactionHandler<T> handler, CompletionHandler<T> completionHandler)
        {
            _instance.RunTransaction((Transaction transaction, ref NSError error) =>
            {
                try
                {
                    var wrapper = new TransactionWrapper(transaction);
                    return new ObjectHolder<T>(handler(wrapper));
                }
                catch (NSErrorException e)
                {
                    error = e.Error;
                }
                catch (Exception e)
                {
                    error = new ExceptionHolder(e);
                }
                return null;
            },
            (NSObject result, NSError error) =>
            {
                T resultObject = default;
                Exception exception = null;

                if (error != null)
                {
                    exception = error is ExceptionHolder exceptionError ? exceptionError.Exception : new CloudFirestoreException(error.LocalizedDescription);
                }
                else
                {
                    if (result is ObjectHolder<T> wrapper)
                    {
                        resultObject = wrapper.Object;
                    }
                }

                completionHandler?.Invoke(resultObject, exception);
            });
        }

        public Task<T> RunTransactionAsync<T>(TransactionHandler<T> handler)
        {
            var tcs = new TaskCompletionSource<T>();

            _instance.RunTransaction((Transaction transaction, ref NSError error) =>
            {
                try
                {
                    var wrapper = new TransactionWrapper(transaction);
                    return new ObjectHolder<T>(handler(wrapper));
                }
                catch (NSErrorException e)
                {
                    error = e.Error;
                }
                catch (Exception e)
                {
                    error = new ExceptionHolder(e);
                }
                return null;
            },
            (NSObject result, NSError error) =>
            {
                if (error != null)
                {
                    if (error is ExceptionHolder exceptionError)
                    {
                        tcs.SetException(exceptionError.Exception);
                    }
                    else
                    {
                        tcs.SetException(new CloudFirestoreException(error.LocalizedDescription));
                    }
                }
                else
                {
                    T resultObject = default(T);
                    if (result is ObjectHolder<T> wrapper)
                    {
                        resultObject = wrapper.Object;
                    }
                    tcs.SetResult(resultObject);
                }
            });

            return tcs.Task;
        }

        public void RunTransaction(TransactionHandler handler, CompletionHandler completionHandler)
        {
            _instance.RunTransaction((Transaction transaction, ref NSError error) =>
            {
                try
                {
                    var wrapper = new TransactionWrapper(transaction);
                    handler(wrapper);
                }
                catch (NSErrorException e)
                {
                    error = e.Error;
                }
                catch (Exception e)
                {
                    error = new ExceptionHolder(e);
                }
                return null;
            },
            (NSObject result, NSError error) =>
            {
                Exception exception = null;

                if (error != null)
                {
                    exception = error is ExceptionHolder exceptionError ? exceptionError.Exception : new CloudFirestoreException(error.LocalizedDescription);
                }

                completionHandler?.Invoke(exception);
            });
        }

        public Task RunTransactionAsync(TransactionHandler handler)
        {
            var tcs = new TaskCompletionSource<bool>();

            _instance.RunTransaction((Transaction transaction, ref NSError error) =>
            {
                try
                {
                    var wrapper = new TransactionWrapper(transaction);
                    handler(wrapper);
                }
                catch (NSErrorException e)
                {
                    error = e.Error;
                }
                catch (Exception e)
                {
                    error = new ExceptionHolder(e);
                }
                return null;
            },
            (NSObject result, NSError error) =>
            {
                if (error != null)
                {
                    if (error is ExceptionHolder exceptionError)
                    {
                        tcs.SetException(exceptionError.Exception);
                    }
                    else
                    {
                        tcs.SetException(new CloudFirestoreException(error.LocalizedDescription));
                    }
                }
                else
                {
                    tcs.SetResult(true);
                }
            });

            return tcs.Task;
        }

        public IWriteBatch CreateBatch()
        {
            var writeBatch = _instance.CreateBatch();
            return new WriteBatchWrapper(writeBatch);
        }
    }
}
