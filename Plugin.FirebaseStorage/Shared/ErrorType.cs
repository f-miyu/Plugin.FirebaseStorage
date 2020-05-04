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
        NotAuthenticated,
        NotAuthorized,
        RetryLimitExceeded,
        InvalidChecksum,
        Canceled,
        DownloadSizeExceeded
    }
}
