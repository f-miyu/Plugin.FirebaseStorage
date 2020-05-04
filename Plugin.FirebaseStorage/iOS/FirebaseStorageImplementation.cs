using System;
using Firebase.Core;

namespace Plugin.FirebaseStorage
{
    public class FirebaseStorageImplementation : IFirebaseStorage
    {
        public IStorage Instance => StorageProvider.Storage;

        public IStorage GetInstance(string appName)
        {
            return StorageProvider.GetStorage(appName);
        }

        public IStorage GetInstanceFromUrl(string url)
        {
            return StorageProvider.GetStorageFromUrl(url);
        }

        public IStorage GetInstance(string appName, string url)
        {
            return StorageProvider.GetStorage(appName, url);
        }
    }
}
