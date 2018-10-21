using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StreamDownloadTaskSnapshotWrapper : IDownloadState
    {
        internal StreamDownloadTask.TaskSnapshot TaskSnapshot { get; }

        public long BytesTransferred => TaskSnapshot.BytesTransferred;

        public long TotalByteCount => TaskSnapshot.TotalByteCount;

        public IStorageReference Reference => TaskSnapshot.Storage != null ? new StorageReferenceWrapper(TaskSnapshot.Storage) : null;

        public StreamDownloadTaskSnapshotWrapper(StreamDownloadTask.TaskSnapshot taskSnapshot)
        {
            TaskSnapshot = taskSnapshot;
        }
    }
}
