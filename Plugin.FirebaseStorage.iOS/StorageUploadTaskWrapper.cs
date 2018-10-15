using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageUploadTaskWrapper : IStorageTask
    {
        internal StorageUploadTask StorageUploadTask { get; }

        public bool IsPaused => StorageUploadTask.Snapshot.Status == StorageTaskStatus.Pause;

        public bool IsInProgress => StorageUploadTask.Snapshot.Status == StorageTaskStatus.Progress;

        public StorageUploadTaskWrapper(StorageUploadTask storageUploadTask)
        {
            StorageUploadTask = storageUploadTask;
        }

        public void Cancel()
        {
            StorageUploadTask.Cancel();
        }

        public void Puase()
        {
            StorageUploadTask.Pause();
        }

        public void Resume()
        {
            StorageUploadTask.Resume();
        }
    }
}
