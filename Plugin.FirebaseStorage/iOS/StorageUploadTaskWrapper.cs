using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageUploadTaskWrapper : IStorageTask
    {
        private readonly StorageUploadTask _storageUploadTask;

        public bool IsPaused => _storageUploadTask.Snapshot.Status == StorageTaskStatus.Pause;

        public bool IsInProgress => _storageUploadTask.Snapshot.Status == StorageTaskStatus.Progress;

        public StorageUploadTaskWrapper(StorageUploadTask storageUploadTask)
        {
            _storageUploadTask = storageUploadTask;
        }

        public void Cancel()
        {
            _storageUploadTask.Cancel();
        }

        public void Puase()
        {
            _storageUploadTask.Pause();
        }

        public void Resume()
        {
            _storageUploadTask.Resume();
        }
    }
}
