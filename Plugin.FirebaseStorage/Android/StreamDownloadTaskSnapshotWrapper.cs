using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StreamDownloadTaskSnapshotWrapper : IDownloadState, IEquatable<StreamDownloadTaskSnapshotWrapper>
    {
        private readonly StreamDownloadTask.TaskSnapshot _taskSnapshot;

        public StreamDownloadTaskSnapshotWrapper(StreamDownloadTask.TaskSnapshot taskSnapshot)
        {
            _taskSnapshot = taskSnapshot ?? throw new ArgumentNullException(nameof(taskSnapshot));
        }

        public long BytesTransferred => _taskSnapshot.BytesTransferred;

        public long TotalByteCount => _taskSnapshot.TotalByteCount;

        public IStorageReference Reference => new StorageReferenceWrapper(_taskSnapshot.Storage);

        public Exception? Error => _taskSnapshot.Error != null ? ExceptionMapper.Map(_taskSnapshot.Error) : null;

        public override bool Equals(object? obj)
        {
            return Equals(obj as StreamDownloadTaskSnapshotWrapper);
        }

        public bool Equals(StreamDownloadTaskSnapshotWrapper? other)
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
