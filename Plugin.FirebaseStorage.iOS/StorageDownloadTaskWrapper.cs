using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageDownloadTaskWrapper : IStorageTask
    {
        private readonly StorageDownloadTask _storageDownloadTask;

        public bool IsPaused => _storageDownloadTask.Snapshot.Status == StorageTaskStatus.Pause;

        public bool IsInProgress => _storageDownloadTask.Snapshot.Status == StorageTaskStatus.Resume || _storageDownloadTask.Snapshot.Status == StorageTaskStatus.Progress;

        public StorageDownloadTaskWrapper(StorageDownloadTask storageDownloadTask)
        {
            _storageDownloadTask = storageDownloadTask;
        }

        public void Cancel()
        {
            _storageDownloadTask.Cancel();
        }

        public void Puase()
        {
            _storageDownloadTask.Pause();
        }

        public void Resume()
        {
            _storageDownloadTask.Resume();
        }
    }
}
