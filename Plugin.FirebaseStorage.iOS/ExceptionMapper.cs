using System;
using Foundation;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    internal static class ExceptionMapper
    {
        public static Exception Map(NSErrorException exception)
        {
            var errorType = ErrorType.Unkown;
            var errorCode = (StorageErrorCode)(long)exception.Error.Code;
            switch (errorCode)
            {
                case StorageErrorCode.BucketNotFound:
                    errorType = ErrorType.BucketNotFound;
                    break;
                case StorageErrorCode.Cancelled:
                    errorType = ErrorType.Canceled;
                    break;
                case StorageErrorCode.NonMatchingChecksum:
                    errorType = ErrorType.InvalidChecksum;
                    break;
                case StorageErrorCode.Unauthenticated:
                    errorType = ErrorType.NotAuthenticated;
                    break;
                case StorageErrorCode.Unauthorized:
                    errorType = ErrorType.NotAuthorized;
                    break;
                case StorageErrorCode.ObjectNotFound:
                    errorType = ErrorType.ObjectNotFound;
                    break;
                case StorageErrorCode.ProjectNotFound:
                    errorType = ErrorType.ProjectNotFound;
                    break;
                case StorageErrorCode.QuotaExceeded:
                    errorType = ErrorType.QuotaExceeded;
                    break;
                case StorageErrorCode.RetryLimitExceeded:
                    errorType = ErrorType.RetryLimitExceeded;
                    break;
                case StorageErrorCode.DownloadSizeExceeded:
                    errorType = ErrorType.DownloadSizeExceeded;
                    break;
            }
            return new FirebaseStorageException(exception.Message, exception, errorType);
        }
    }
}
