using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class UploadTaskSnapshotWrapper : IUploadTaskSnapshot
    {
        internal StorageTaskSnapshot StorageTaskSnapshot { get; }

        public UploadTaskSnapshotWrapper(StorageTaskSnapshot storageTaskSnapshot)
        {
            StorageTaskSnapshot = storageTaskSnapshot;
        }

        public long BytesTransferred => StorageTaskSnapshot.Progress.CompletedUnitCount;

        public long TotalByteCount => StorageTaskSnapshot.Progress.TotalUnitCount;
    }
}
