using System;
namespace Plugin.FirebaseStorage
{
    public enum ErrorType
    {
        Unkown,
        ObjectNotFound,
        BucketNotFound,
        ProjectNotFound,
        QuotaExceeded,
        Unauthenticated,
        Unauthorized,
        RetryLimitExceeded,
        NonMatchingChecksum,
        Canceled,
        DownloadSizeExceeded
    }
}
