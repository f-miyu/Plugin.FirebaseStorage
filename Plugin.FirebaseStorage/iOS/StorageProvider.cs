using System;
using System.Collections.Concurrent;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    internal static class StorageProvider
    {
        public static StorageWrapper Storage => new StorageWrapper(Firebase.Storage.Storage.DefaultInstance);

        public static StorageWrapper GetStorage(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return GetStorage(Firebase.Storage.Storage.From(app));
        }

        public static StorageWrapper GetStorageFromUrl(string url)
        {
            return GetStorage(Firebase.Storage.Storage.From(url));
        }

        public static StorageWrapper GetStorage(string appName, string url)
        {
            var app = Firebase.Core.App.From(appName);
            return GetStorage(Firebase.Storage.Storage.From(app, url));
        }

        public static StorageWrapper GetStorage(Storage storage)
        {
            return new StorageWrapper(storage);
        }
    }
}
