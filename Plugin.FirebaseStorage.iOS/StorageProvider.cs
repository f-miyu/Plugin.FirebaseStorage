using System;
using System.Collections.Concurrent;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    internal static class StorageProvider
    {
        private static ConcurrentDictionary<Storage, Lazy<StorageWrapper>> _storages = new ConcurrentDictionary<Storage, Lazy<StorageWrapper>>();

        public static StorageWrapper Storage => _storages.GetOrAdd(Firebase.Storage.Storage.DefaultInstance, key => new Lazy<StorageWrapper>(() => new StorageWrapper(key))).Value;

        public static StorageWrapper GetStorage(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return _storages.GetOrAdd(Firebase.Storage.Storage.From(app), key => new Lazy<StorageWrapper>(() => new StorageWrapper(key))).Value;
        }

        public static StorageWrapper GetStorageFromUrl(string url)
        {
            return _storages.GetOrAdd(Firebase.Storage.Storage.From(url), key => new Lazy<StorageWrapper>(() => new StorageWrapper(key))).Value;
        }

        public static StorageWrapper GetStorage(string appName, string url)
        {
            var app = Firebase.Core.App.From(appName);
            return _storages.GetOrAdd(Firebase.Storage.Storage.From(app, url), key => new Lazy<StorageWrapper>(() => new StorageWrapper(key))).Value;
        }

        public static StorageWrapper GetStorage(Storage storage)
        {
            return _storages.GetOrAdd(storage, key => new Lazy<StorageWrapper>(() => new StorageWrapper(key))).Value;
        }
    }
}
