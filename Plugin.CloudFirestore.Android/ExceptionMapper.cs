using System;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    internal static class ExceptionMapper
    {
        public static Exception Map(Exception exception)
        {
            var errorType = ErrorType.Unknown;

            if (exception is FirebaseFirestoreException firebaseFirestoreException)
            {
                var code = firebaseFirestoreException.GetCode();

                if (code == FirebaseFirestoreException.Code.Ok)
                {
                    errorType = ErrorType.OK;
                }
                else if (code == FirebaseFirestoreException.Code.Cancelled)
                {
                    errorType = ErrorType.Cancelled;
                }
                else if (code == FirebaseFirestoreException.Code.Unknown)
                {
                    errorType = ErrorType.Unknown;
                }
                else if (code == FirebaseFirestoreException.Code.InvalidArgument)
                {
                    errorType = ErrorType.InvalidArgument;
                }
                else if (code == FirebaseFirestoreException.Code.DeadlineExceeded)
                {
                    errorType = ErrorType.DeadlineExceeded;
                }
                else if (code == FirebaseFirestoreException.Code.NotFound)
                {
                    errorType = ErrorType.NotFound;
                }
                else if (code == FirebaseFirestoreException.Code.AlreadyExists)
                {
                    errorType = ErrorType.AlreadyExists;
                }
                else if (code == FirebaseFirestoreException.Code.PermissionDenied)
                {
                    errorType = ErrorType.PermissionDenied;
                }
                else if (code == FirebaseFirestoreException.Code.ResourceExhausted)
                {
                    errorType = ErrorType.ResourceExhausted;
                }
                else if (code == FirebaseFirestoreException.Code.FailedPrecondition)
                {
                    errorType = ErrorType.FailedPrecondition;
                }
                else if (code == FirebaseFirestoreException.Code.Aborted)
                {
                    errorType = ErrorType.Aborted;
                }
                else if (code == FirebaseFirestoreException.Code.OutOfRange)
                {
                    errorType = ErrorType.OutOfRange;
                }
                else if (code == FirebaseFirestoreException.Code.Unimplemented)
                {
                    errorType = ErrorType.Unimplemented;
                }
                else if (code == FirebaseFirestoreException.Code.Internal)
                {
                    errorType = ErrorType.Internal;
                }
                else if (code == FirebaseFirestoreException.Code.Unavailable)
                {
                    errorType = ErrorType.Unavailable;
                }
                else if (code == FirebaseFirestoreException.Code.DataLoss)
                {
                    errorType = ErrorType.DataLoss;
                }
                else if (code == FirebaseFirestoreException.Code.Unauthenticated)
                {
                    errorType = ErrorType.Unauthenticated;
                }
            }

            return new CloudFirestoreException(exception.Message, errorType, exception);
        }
    }
}
