using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageUploadTaskWrapper : IStorageTask, IEquatable<StorageUploadTaskWrapper>
    {
        private readonly StorageUploadTask _storageUploadTask;

        public StorageUploadTaskWrapper(StorageUploadTask storageUploadTask)
        {
            _storageUploadTask = storageUploadTask ?? throw new ArgumentNullException(nameof(storageUploadTask));
        }

        public bool IsPaused => _storageUploadTask.Snapshot.Status == StorageTaskStatus.Pause;

        public bool IsInProgress => _storageUploadTask.Snapshot.Status == StorageTaskStatus.Resume
            || _storageUploadTask.Snapshot.Status == StorageTaskStatus.Progress;

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

        public override bool Equals(object? obj)
        {
            return Equals(obj as StorageUploadTaskWrapper);
        }

        public bool Equals(StorageUploadTaskWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_storageUploadTask, other._storageUploadTask)) return true;
            return _storageUploadTask.Equals(other._storageUploadTask);
        }

        public override int GetHashCode()
        {
            return _storageUploadTask.GetHashCode();
        }
    }
}
