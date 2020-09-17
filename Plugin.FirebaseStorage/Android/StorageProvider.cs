using System;
using System.Collections.Concurrent;

namespace Plugin.FirebaseStorage
{
    internal static class StorageProvider
    {
        public static StorageWrapper Storage => new StorageWrapper(Firebase.Storage.FirebaseStorage.Instance);

        public static StorageWrapper GetStorage(string appName)
        {
            var app = Firebase.FirebaseApp.GetInstance(appName);
            return GetStorage(Firebase.Storage.FirebaseStorage.GetInstance(app));
        }

        public static StorageWrapper GetStorageFromUrl(string url)
        {
            return GetStorage(Firebase.Storage.FirebaseStorage.GetInstance(url));
        }

        public static StorageWrapper GetStorage(string appName, string url)
        {
            var app = Firebase.FirebaseApp.GetInstance(appName);
            return GetStorage(Firebase.Storage.FirebaseStorage.GetInstance(app, url));
        }

        public static StorageWrapper GetStorage(Firebase.Storage.FirebaseStorage storage)
        {
            return new StorageWrapper(storage);
        }
    }
}
