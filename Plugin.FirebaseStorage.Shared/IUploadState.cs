using System;

namespace Plugin.FirebaseStorage
{
    public interface IUploadTaskSnapshot
    {
        long BytesTransferred { get; }
        long TotalByteCount { get; }
    }
}
