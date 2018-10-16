using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class UploadTaskWrapper : IStorageTask
    {
        internal StorageTask StorageTask { get; }

        public bool IsPaused => StorageTask.IsPaused;

        public bool IsInProgress => StorageTask.IsInProgress;

        public UploadTaskWrapper(StorageTask storageTask)
        {
            StorageTask = storageTask;
        }

        public void Cancel()
        {
            StorageTask.Cancel();
        }

        public void Puase()
        {
            StorageTask.Pause();
        }

        public void Resume()
        {
            StorageTask.Resume();
        }
    }
}
