using System;
namespace Plugin.FirebaseStorage
{
    public interface IDownloadState
    {
        long BytesTransferred { get; }
        long TotalByteCount { get; }
        IStorageReference Reference { get; }
        Exception? Error { get; }
    }
}
