using System;
using System.Threading.Tasks;
using Firebase.CloudFirestore;
using Foundation;

namespace Plugin.CloudFirestore
{
    public class FirestoreWrapper : IFirestore
    {
        public bool PersistenceEnabled
        {
            get => _firestore.Settings.PersistenceEnabled;
            set
            {
                var settings = new FirestoreSettings();
                settings.PersistenceEnabled = value;
                _firestore.Settings = settings;
            }
        }

        private readonly Firestore _firestore;

        public FirestoreWrapper(Firestore firestore)
        {
            _firestore = firestore;
        }

        public ICollectionReference GetCollection(string collectionPath)
        {
            var collectionReference = _firestore.GetCollection(collectionPath);
            return new CollectionReferenceWrapper(collectionReference);
        }

        public IDocumentReference GetDocument(string documentPath)
        {
            var documentReference = _firestore.GetDocument(documentPath);
            return new DocumentReferenceWrapper(documentReference);
        }

        public void RunTransaction<T>(TransactionHandler<T> handler, CompletionHandler<T> completionHandler)
        {
            _firestore.RunTransaction((Transaction transaction, ref NSError error) =>
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
                    error = new ExceptionError(e);
                }
                return null;
            },
            (NSObject result, NSError error) =>
            {
                T resultObject = default;
                Exception exception = null;

                if (error != null)
                {
                    exception = error is ExceptionError exceptionError ? exceptionError.Exception : ExceptionMapper.Map(error);
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

            _firestore.RunTransaction((Transaction transaction, ref NSError error) =>
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
                    error = new ExceptionError(e);
                }
                return null;
            },
            (NSObject result, NSError error) =>
            {
                if (error != null)
                {
                    if (error is ExceptionError exceptionError)
                    {
                        tcs.SetException(exceptionError.Exception);
                    }
                    else
                    {
                        tcs.SetException(ExceptionMapper.Map(error));
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
            _firestore.RunTransaction((Transaction transaction, ref NSError error) =>
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
                    error = new ExceptionError(e);
                }
                return null;
            },
            (NSObject result, NSError error) =>
            {
                Exception exception = null;

                if (error != null)
                {
                    exception = error is ExceptionError exceptionError ? exceptionError.Exception : ExceptionMapper.Map(error);
                }

                completionHandler?.Invoke(exception);
            });
        }

        public Task RunTransactionAsync(TransactionHandler handler)
        {
            var tcs = new TaskCompletionSource<bool>();

            _firestore.RunTransaction((Transaction transaction, ref NSError error) =>
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
                    error = new ExceptionError(e);
                }
                return null;
            },
            (NSObject result, NSError error) =>
            {
                if (error != null)
                {
                    if (error is ExceptionError exceptionError)
                    {
                        tcs.SetException(exceptionError.Exception);
                    }
                    else
                    {
                        tcs.SetException(ExceptionMapper.Map(error));
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
            var writeBatch = _firestore.CreateBatch();
            return new WriteBatchWrapper(writeBatch);
        }
    }
}
