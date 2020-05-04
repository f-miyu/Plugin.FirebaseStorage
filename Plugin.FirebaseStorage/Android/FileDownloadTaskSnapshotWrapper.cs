using System;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class FileDownloadTaskSnapshotWrapper : IDownloadState
    {
        private readonly FileDownloadTask.TaskSnapshot _taskSnapshot;

        public long BytesTransferred => _taskSnapshot.BytesTransferred;

        public long TotalByteCount => _taskSnapshot.TotalByteCount;

        public IStorageReference Reference => _taskSnapshot.Storage != null ? new StorageReferenceWrapper(_taskSnapshot.Storage) : null;

        public FileDownloadTaskSnapshotWrapper(FileDownloadTask.TaskSnapshot taskSnapshot)
        {
            _taskSnapshot = taskSnapshot;
        }
    }
}
