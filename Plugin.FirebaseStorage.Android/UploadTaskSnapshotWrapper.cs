using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class UploadTaskSnapshotWrapper : IUploadState
    {
        internal UploadTask.TaskSnapshot TaskSnapshot { get; }

        public long BytesTransferred => TaskSnapshot.BytesTransferred;

        public long TotalByteCount => TaskSnapshot.TotalByteCount;

        public IStorageReference Reference => TaskSnapshot.Storage != null ? new StorageReferenceWrapper(TaskSnapshot.Storage) : null;

        public IStorageMetadata Metadata => TaskSnapshot.Metadata != null ? new StorageMetadataWrapper(TaskSnapshot.Metadata) : null;

        public UploadTaskSnapshotWrapper(UploadTask.TaskSnapshot taskSnapshot)
        {
            TaskSnapshot = taskSnapshot;
        }
    }
}
