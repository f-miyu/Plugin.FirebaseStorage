using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class UploadTaskSnapshotWrapper : IUploadState
    {
        private readonly UploadTask.TaskSnapshot _taskSnapshot;

        public long BytesTransferred => _taskSnapshot.BytesTransferred;

        public long TotalByteCount => _taskSnapshot.TotalByteCount;

        public IStorageReference Reference => _taskSnapshot.Storage != null ? new StorageReferenceWrapper(_taskSnapshot.Storage) : null;

        public IStorageMetadata Metadata => _taskSnapshot.Metadata != null ? new StorageMetadataWrapper(_taskSnapshot.Metadata) : null;

        public UploadTaskSnapshotWrapper(UploadTask.TaskSnapshot taskSnapshot)
        {
            _taskSnapshot = taskSnapshot;
        }
    }
}
