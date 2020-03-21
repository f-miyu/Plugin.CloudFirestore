using System;
using Foundation;
using Firebase.CloudFirestore;
namespace Plugin.CloudFirestore
{
    internal static class ExceptionMapper
    {
        public static Exception Map(NSErrorException exception)
        {
            var errorType = ErrorType.Unknown;
            var errorCode = (FirestoreErrorCode)(long)exception.Error.Code;

            switch (errorCode)
            {
                case FirestoreErrorCode.Ok:
                    errorType = ErrorType.OK;
                    break;
                case FirestoreErrorCode.Cancelled:
                    errorType = ErrorType.Cancelled;
                    break;
                case FirestoreErrorCode.Unknown:
                    errorType = ErrorType.Unknown;
                    break;
                case FirestoreErrorCode.InvalidArgument:
                    errorType = ErrorType.InvalidArgument;
                    break;
                case FirestoreErrorCode.DeadlineExceeded:
                    errorType = ErrorType.DeadlineExceeded;
                    break;
                case FirestoreErrorCode.NotFound:
                    errorType = ErrorType.NotFound;
                    break;
                case FirestoreErrorCode.AlreadyExists:
                    errorType = ErrorType.AlreadyExists;
                    break;
                case FirestoreErrorCode.PermissionDenied:
                    errorType = ErrorType.PermissionDenied;
                    break;
                case FirestoreErrorCode.ResourceExhausted:
                    errorType = ErrorType.ResourceExhausted;
                    break;
                case FirestoreErrorCode.FailedPrecondition:
                    errorType = ErrorType.FailedPrecondition;
                    break;
                case FirestoreErrorCode.Aborted:
                    errorType = ErrorType.Aborted;
                    break;
                case FirestoreErrorCode.OutOfRange:
                    errorType = ErrorType.OutOfRange;
                    break;
                case FirestoreErrorCode.Unimplemented:
                    errorType = ErrorType.Unimplemented;
                    break;
                case FirestoreErrorCode.Internal:
                    errorType = ErrorType.Internal;
                    break;
                case FirestoreErrorCode.Unavailable:
                    errorType = ErrorType.Unavailable;
                    break;
                case FirestoreErrorCode.DataLoss:
                    errorType = ErrorType.DataLoss;
                    break;
                case FirestoreErrorCode.Unauthenticated:
                    errorType = ErrorType.Unauthenticated;
                    break;
            }

            return new CloudFirestoreException(exception.Error.LocalizedDescription, errorType, exception);
        }

        public static Exception Map(NSError error)
        {
            return Map(new NSErrorException(error));
        }
    }
}
