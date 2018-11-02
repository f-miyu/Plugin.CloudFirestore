using System;
namespace Plugin.CloudFirestore
{
    public enum ErrorType
    {
        OK,
        Cancelled,
        Unknown,
        InvalidArgument,
        DeadlineExceeded,
        NotFound,
        AlreadyExists,
        PermissionDenied,
        ResourceExhausted,
        FailedPrecondition,
        Aborted,
        OutOfRange,
        Unimplemented,
        Internal,
        Unavailable,
        DataLoss,
        Unauthenticated
    }
}
