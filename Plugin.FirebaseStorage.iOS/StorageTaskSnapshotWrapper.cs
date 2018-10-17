using System;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class StorageTaskSnapshotWrapper : IUploadState, IDownloadState
    {
        internal StorageTaskSnapshot StorageTaskSnapshot { get; }

        public long BytesTransferred => StorageTaskSnapshot.Progress.CompletedUnitCount;

        public long TotalByteCount => StorageTaskSnapshot.Progress.TotalUnitCount;

        public IStorageReference Reference => new StorageReferenceWrapper(StorageTaskSnapshot.Reference);

        public StorageTaskSnapshotWrapper(StorageTaskSnapshot storageTaskSnapshot)
        {
            StorageTaskSnapshot = storageTaskSnapshot;

            var task = StorageTaskSnapshot.GetTask<StorageDownloadTask>();
        }
    }
}
