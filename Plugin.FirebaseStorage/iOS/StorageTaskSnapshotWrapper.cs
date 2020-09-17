using System;
using Firebase.Storage;
using Foundation;

namespace Plugin.FirebaseStorage
{
    public class StorageTaskSnapshotWrapper : IUploadState, IDownloadState, IEquatable<StorageTaskSnapshotWrapper>
    {
        private readonly StorageTaskSnapshot _storageTaskSnapshot;

        public StorageTaskSnapshotWrapper(StorageTaskSnapshot storageTaskSnapshot)
        {
            _storageTaskSnapshot = storageTaskSnapshot ?? throw new ArgumentNullException(nameof(storageTaskSnapshot));
        }

        public long BytesTransferred => _storageTaskSnapshot.Progress?.CompletedUnitCount ?? 0;

        public long TotalByteCount => _storageTaskSnapshot.Progress?.TotalUnitCount ?? -1;

        public IStorageReference Reference => new StorageReferenceWrapper(_storageTaskSnapshot.Reference);

        public IStorageMetadata? Metadata => _storageTaskSnapshot.Metadata != null ? new StorageMetadataWrapper(_storageTaskSnapshot.Metadata) : null;

        public Uri? UploadSessionUri { get; }

        public Exception? Error => _storageTaskSnapshot.Error != null ? ExceptionMapper.Map(new NSErrorException(_storageTaskSnapshot.Error)) : null;

        public override bool Equals(object? obj)
        {
            return Equals(obj as StorageTaskSnapshotWrapper);
        }

        public bool Equals(StorageTaskSnapshotWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_storageTaskSnapshot, other._storageTaskSnapshot)) return true;
            return _storageTaskSnapshot.Equals(other._storageTaskSnapshot);
        }

        public override int GetHashCode()
        {
            return _storageTaskSnapshot.GetHashCode();
        }
    }
}
