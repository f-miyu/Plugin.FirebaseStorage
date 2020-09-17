using System;
namespace Plugin.FirebaseStorage
{
    public class StorageWrapper : IStorage, IEquatable<StorageWrapper>
    {
        private readonly Firebase.Storage.FirebaseStorage _storage;

        public StorageWrapper(Firebase.Storage.FirebaseStorage storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public IStorageReference RootReference => new StorageReferenceWrapper(_storage.Reference);

        public TimeSpan MaxDownloadRetryTime
        {
            get => TimeSpan.FromMilliseconds(_storage.MaxDownloadRetryTimeMillis);
            set => _storage.MaxDownloadRetryTimeMillis = (long)value.TotalMilliseconds;
        }

        public TimeSpan MaxOperationRetryTime
        {
            get => TimeSpan.FromMilliseconds(_storage.MaxOperationRetryTimeMillis);
            set => _storage.MaxOperationRetryTimeMillis = (long)value.TotalMilliseconds;
        }

        public TimeSpan MaxUploadRetryTime
        {
            get => TimeSpan.FromMilliseconds(_storage.MaxUploadRetryTimeMillis);
            set => _storage.MaxUploadRetryTimeMillis = (long)value.TotalMilliseconds;
        }

        public IStorageReference GetReferenceFromPath(string path)
        {
            var reference = _storage.GetReference(path);
            return new StorageReferenceWrapper(reference);
        }

        public IStorageReference GetReferenceFromUrl(string url)
        {
            var reference = _storage.GetReferenceFromUrl(url);
            return new StorageReferenceWrapper(reference);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as StorageWrapper);
        }

        public bool Equals(StorageWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_storage, other._storage)) return true;
            return _storage.Equals(other._storage);
        }

        public override int GetHashCode()
        {
            return _storage.GetHashCode();
        }
    }
}
