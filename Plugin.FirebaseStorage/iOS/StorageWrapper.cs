using System;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class StorageWrapper : IStorage
    {
        private readonly Storage _storage;

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

        public StorageWrapper(Storage storage)
        {
            _storage = storage;
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
    }
}
