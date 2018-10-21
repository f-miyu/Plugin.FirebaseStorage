using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageDownloadTaskWrapper : IStorageTask
    {
        internal StorageDownloadTask StorageDownloadTask { get; }

        public bool IsPaused => StorageDownloadTask.Snapshot.Status == StorageTaskStatus.Pause;

        public bool IsInProgress => StorageDownloadTask.Snapshot.Status == StorageTaskStatus.Resume || StorageDownloadTask.Snapshot.Status == StorageTaskStatus.Progress;

        public StorageDownloadTaskWrapper(StorageDownloadTask storageDownloadTask)
        {
            StorageDownloadTask = storageDownloadTask;
        }

        public void Cancel()
        {
            StorageDownloadTask.Cancel();
        }

        public void Puase()
        {
            StorageDownloadTask.Pause();
        }

        public void Resume()
        {
            StorageDownloadTask.Resume();
        }
    }
}
