using System;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class FileDownloadTaskSnapshotWrapper : IDownloadState, IEquatable<FileDownloadTaskSnapshotWrapper>
    {
        private readonly FileDownloadTask.TaskSnapshot _taskSnapshot;

        public FileDownloadTaskSnapshotWrapper(FileDownloadTask.TaskSnapshot taskSnapshot)
        {
            _taskSnapshot = taskSnapshot ?? throw new ArgumentNullException(nameof(taskSnapshot));
        }

        public long BytesTransferred => _taskSnapshot.BytesTransferred;

        public long TotalByteCount => _taskSnapshot.TotalByteCount;

        public IStorageReference Reference => new StorageReferenceWrapper(_taskSnapshot.Storage);

        public Exception? Error => _taskSnapshot.Error != null ? ExceptionMapper.Map(_taskSnapshot.Error) : null;

        public override bool Equals(object? obj)
        {
            return Equals(obj as FileDownloadTaskSnapshotWrapper);
        }

        public bool Equals(FileDownloadTaskSnapshotWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_taskSnapshot, other._taskSnapshot)) return true;
            return _taskSnapshot.Equals(other._taskSnapshot);
        }

        public override int GetHashCode()
        {
            return _taskSnapshot.GetHashCode();
        }
    }
}
