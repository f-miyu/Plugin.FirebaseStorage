using System;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class StorageWrapper : IStorage, IEquatable<StorageWrapper>
    {
        private readonly Storage _storage;

        public StorageWrapper(Storage storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public IStorageReference RootReference => new StorageReferenceWrapper(_storage.GetRootReference());

        public TimeSpan MaxDownloadRetryTime
        {
            get => TimeSpan.FromSeconds(_storage.MaxDownloadRetryTime);
            set => _storage.MaxDownloadRetryTime = value.TotalSeconds;
        }

        public TimeSpan MaxOperationRetryTime
        {
            get => TimeSpan.FromSeconds(_storage.MaxOperationRetryTime);
            set => _storage.MaxOperationRetryTime = value.TotalSeconds;
        }

        public TimeSpan MaxUploadRetryTime
        {
            get => TimeSpan.FromSeconds(_storage.MaxUploadRetryTime);
            set => _storage.MaxUploadRetryTime = value.TotalSeconds;
        }

        public IStorageReference GetReferenceFromPath(string path)
        {
            var reference = _storage.GetReferenceFromPath(path);
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
