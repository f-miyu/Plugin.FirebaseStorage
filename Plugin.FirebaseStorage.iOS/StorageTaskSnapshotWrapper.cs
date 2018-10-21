using System;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class StorageTaskSnapshotWrapper : IUploadState, IDownloadState
    {
        internal StorageTaskSnapshot StorageTaskSnapshot { get; }

        public long BytesTransferred => StorageTaskSnapshot.Progress.CompletedUnitCount;

        public long TotalByteCount => StorageTaskSnapshot.Progress.TotalUnitCount;

        public IStorageReference Reference => StorageTaskSnapshot.Reference != null ? new StorageReferenceWrapper(StorageTaskSnapshot.Reference) : null;

        public IStorageMetadata Metadata => StorageTaskSnapshot.Metadata != null ? new StorageMetadataWrapper(StorageTaskSnapshot.Metadata) : null;

        public StorageTaskSnapshotWrapper(StorageTaskSnapshot storageTaskSnapshot)
        {
            StorageTaskSnapshot = storageTaskSnapshot;
        }
    }
}
