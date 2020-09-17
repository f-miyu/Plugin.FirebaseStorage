using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class UploadTaskSnapshotWrapper : IUploadState, IEquatable<UploadTaskSnapshotWrapper>
    {
        private readonly UploadTask.TaskSnapshot _taskSnapshot;

        public UploadTaskSnapshotWrapper(UploadTask.TaskSnapshot taskSnapshot)
        {
            _taskSnapshot = taskSnapshot ?? throw new ArgumentNullException(nameof(taskSnapshot));
        }

        public long BytesTransferred => _taskSnapshot.BytesTransferred;

        public long TotalByteCount => _taskSnapshot.TotalByteCount;

        public IStorageReference Reference => new StorageReferenceWrapper(_taskSnapshot.Storage);

        public IStorageMetadata? Metadata => _taskSnapshot.Metadata != null ? new StorageMetadataWrapper(_taskSnapshot.Metadata) : null;

        public Uri? UploadSessionUri => _taskSnapshot.UploadSessionUri != null ? new Uri(_taskSnapshot.UploadSessionUri.ToString()) : null;

        public Exception? Error => _taskSnapshot.Error != null ? ExceptionMapper.Map(_taskSnapshot.Error) : null;

        public override bool Equals(object? obj)
        {
            return Equals(obj as UploadTaskSnapshotWrapper);
        }

        public bool Equals(UploadTaskSnapshotWrapper? other)
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
