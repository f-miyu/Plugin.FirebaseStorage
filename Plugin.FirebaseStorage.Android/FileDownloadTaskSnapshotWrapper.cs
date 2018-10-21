using System;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class FileDownloadTaskSnapshotWrapper : IDownloadState
    {
        internal FileDownloadTask.TaskSnapshot TaskSnapshot { get; }

        public long BytesTransferred => TaskSnapshot.BytesTransferred;

        public long TotalByteCount => TaskSnapshot.TotalByteCount;

        public IStorageReference Reference => TaskSnapshot.Storage != null ? new StorageReferenceWrapper(TaskSnapshot.Storage) : null;

        public FileDownloadTaskSnapshotWrapper(FileDownloadTask.TaskSnapshot taskSnapshot)
        {
            TaskSnapshot = taskSnapshot;
        }
    }
}
