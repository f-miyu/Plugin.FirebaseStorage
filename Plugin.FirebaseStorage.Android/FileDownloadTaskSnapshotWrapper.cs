using System;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class FileDownloadTaskSnapshotWrapper : IDownloadState
    {
        internal FileDownloadTask.TaskSnapshot TaskSnapshot { get; }

        public long BytesTransferred => TaskSnapshot.BytesTransferred;

        public long TotalByteCount => TaskSnapshot.TotalByteCount;

        public IStorageReference Reference => new StorageReferenceWrapper(TaskSnapshot.Storage);

        public FileDownloadTaskSnapshotWrapper(FileDownloadTask.TaskSnapshot taskSnapshot)
        {
            TaskSnapshot = taskSnapshot;
        }
    }
}
