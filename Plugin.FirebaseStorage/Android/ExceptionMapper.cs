using System;
using Firebase;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    internal static class ExceptionMapper
    {
        public static Exception Map(Exception exception)
        {
            var errorType = ErrorType.Unkown;
            if (exception is StorageException storageException)
            {
                switch (storageException.ErrorCode)
                {
                    case StorageException.ErrorBucketNotFound:
                        errorType = ErrorType.BucketNotFound;
                        break;
                    case StorageException.ErrorCanceled:
                        errorType = ErrorType.Canceled;
                        break;
                    case StorageException.ErrorInvalidChecksum:
                        errorType = ErrorType.InvalidChecksum;
                        break;
                    case StorageException.ErrorNotAuthenticated:
                        errorType = ErrorType.NotAuthenticated;
                        break;
                    case StorageException.ErrorNotAuthorized:
                        errorType = ErrorType.NotAuthorized;
                        break;
                    case StorageException.ErrorObjectNotFound:
                        errorType = ErrorType.ObjectNotFound;
                        break;
                    case StorageException.ErrorProjectNotFound:
                        errorType = ErrorType.ProjectNotFound;
                        break;
                    case StorageException.ErrorQuotaExceeded:
                        errorType = ErrorType.QuotaExceeded;
                        break;
                    case StorageException.ErrorRetryLimitExceeded:
                        errorType = ErrorType.RetryLimitExceeded;
                        break;
                }
            }

            return new FirebaseStorageException(exception.Message, exception, errorType);
        }
    }
}