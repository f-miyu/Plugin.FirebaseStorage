using System;
using System.Collections.Concurrent;

namespace Plugin.FirebaseStorage
{
    internal static class StorageProvider
    {
        private static ConcurrentDictionary<Firebase.Storage.FirebaseStorage, Lazy<StorageWrapper>> _storages = new ConcurrentDictionary<Firebase.Storage.FirebaseStorage, Lazy<StorageWrapper>>();

        public static StorageWrapper Storage => _storages.GetOrAdd(Firebase.Storage.FirebaseStorage.Instance, key => new Lazy<StorageWrapper>(() => new StorageWrapper(key))).Value;

        public static StorageWrapper GetStorage(string appName)
        {
            var app = Firebase.FirebaseApp.GetInstance(appName);
            return _storages.GetOrAdd(Firebase.Storage.FirebaseStorage.GetInstance(app), key => new Lazy<StorageWrapper>(() => new StorageWrapper(key))).Value;
        }

        public static StorageWrapper GetStorageFromUrl(string url)
        {
            return _storages.GetOrAdd(Firebase.Storage.FirebaseStorage.GetInstance(url), key => new Lazy<StorageWrapper>(() => new StorageWrapper(key))).Value;
        }

        public static StorageWrapper GetStorage(string appName, string url)
        {
            var app = Firebase.FirebaseApp.GetInstance(appName);
            return _storages.GetOrAdd(Firebase.Storage.FirebaseStorage.GetInstance(app, url), key => new Lazy<StorageWrapper>(() => new StorageWrapper(key))).Value;
        }

        public static StorageWrapper GetStorage(Firebase.Storage.FirebaseStorage storage)
        {
            return _storages.GetOrAdd(storage, key => new Lazy<StorageWrapper>(() => new StorageWrapper(key))).Value;
        }
    }
}
