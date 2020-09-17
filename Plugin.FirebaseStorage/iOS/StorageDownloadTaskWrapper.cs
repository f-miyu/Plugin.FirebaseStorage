using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageDownloadTaskWrapper : IStorageTask, IEquatable<StorageDownloadTaskWrapper>
    {
        private readonly StorageDownloadTask _storageDownloadTask;

        public StorageDownloadTaskWrapper(StorageDownloadTask storageDownloadTask)
        {
            _storageDownloadTask = storageDownloadTask ?? throw new ArgumentNullException(nameof(storageDownloadTask));
        }

        public bool IsPaused => _storageDownloadTask.Snapshot.Status == StorageTaskStatus.Pause;

        public bool IsInProgress => _storageDownloadTask.Snapshot.Status == StorageTaskStatus.Resume
            || _storageDownloadTask.Snapshot.Status == StorageTaskStatus.Progress;

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

        public override bool Equals(object? obj)
        {
            return Equals(obj as StorageDownloadTaskWrapper);
        }

        public bool Equals(StorageDownloadTaskWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_storageDownloadTask, other._storageDownloadTask)) return true;
            return _storageDownloadTask.Equals(other._storageDownloadTask);
        }

        public override int GetHashCode()
        {
            return _storageDownloadTask.GetHashCode();
        }
    }
}
