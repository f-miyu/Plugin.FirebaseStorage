using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageTaskWrapper : IStorageTask
    {
        internal StorageTask StorageTask { get; }

        public bool IsPaused => StorageTask.IsPaused;

        public bool IsInProgress => StorageTask.IsInProgress;

        public StorageTaskWrapper(StorageTask storageTask)
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
