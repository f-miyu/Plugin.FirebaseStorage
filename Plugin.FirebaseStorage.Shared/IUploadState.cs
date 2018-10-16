using System;

namespace Plugin.FirebaseStorage
{
    public interface IUploadState
    {
        long BytesTransferred { get; }
        long TotalByteCount { get; }
    }
}
