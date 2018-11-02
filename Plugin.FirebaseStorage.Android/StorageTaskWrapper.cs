using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageTaskWrapper : IStorageTask
    {
        private readonly StorageTask _storageTask;

        public bool IsPaused => _storageTask.IsPaused;

        public bool IsInProgress => _storageTask.IsInProgress;

        public StorageTaskWrapper(StorageTask storageTask)
        {
            _storageTask = storageTask;
        }

        public void Cancel()
        {
            _storageTask.Cancel();
        }

        public void Puase()
        {
            _storageTask.Pause();
        }

        public void Resume()
        {
            _storageTask.Resume();
        }
    }
}
