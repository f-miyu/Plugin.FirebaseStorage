using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageTaskWrapper : IStorageTask, IEquatable<StorageTaskWrapper>
    {
        private readonly StorageTask _storageTask;

        public StorageTaskWrapper(StorageTask storageTask)
        {
            _storageTask = storageTask ?? throw new ArgumentNullException(nameof(storageTask));
        }

        public bool IsPaused => _storageTask.IsPaused;

        public bool IsInProgress => _storageTask.IsInProgress;

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

        public override bool Equals(object? obj)
        {
            return Equals(obj as StorageTaskWrapper);
        }

        public bool Equals(StorageTaskWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_storageTask, other._storageTask)) return true;
            return _storageTask.Equals(other._storageTask);
        }

        public override int GetHashCode()
        {
            return _storageTask.GetHashCode();
        }
    }
}
