using System;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class StorageTaskSnapshotWrapper : IUploadState, IDownloadState
    {
        private readonly StorageTaskSnapshot _storageTaskSnapshot;

        public long BytesTransferred => _storageTaskSnapshot.Progress.CompletedUnitCount;

        public long TotalByteCount => _storageTaskSnapshot.Progress.TotalUnitCount;

        public IStorageReference Reference => _storageTaskSnapshot.Reference != null ? new StorageReferenceWrapper(_storageTaskSnapshot.Reference) : null;

        public IStorageMetadata Metadata => _storageTaskSnapshot.Metadata != null ? new StorageMetadataWrapper(_storageTaskSnapshot.Metadata) : null;

        public StorageTaskSnapshotWrapper(StorageTaskSnapshot storageTaskSnapshot)
        {
            _storageTaskSnapshot = storageTaskSnapshot;
        }
    }
}
