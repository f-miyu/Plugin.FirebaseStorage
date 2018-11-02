using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StreamDownloadTaskSnapshotWrapper : IDownloadState
    {
        private readonly StreamDownloadTask.TaskSnapshot _taskSnapshot;

        public long BytesTransferred => _taskSnapshot.BytesTransferred;

        public long TotalByteCount => _taskSnapshot.TotalByteCount;

        public IStorageReference Reference => _taskSnapshot.Storage != null ? new StorageReferenceWrapper(_taskSnapshot.Storage) : null;

        public StreamDownloadTaskSnapshotWrapper(StreamDownloadTask.TaskSnapshot taskSnapshot)
        {
            _taskSnapshot = taskSnapshot;
        }
    }
}
