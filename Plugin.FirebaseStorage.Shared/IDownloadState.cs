﻿using System;
namespace Plugin.FirebaseStorage
{
    public interface IDownloadTaskSnapshot
    {
        long BytesTransferred { get; }
        long TotalByteCount { get; }
        IStorageReference Reference { get; }
    }
}
