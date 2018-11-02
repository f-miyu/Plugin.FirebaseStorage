using System;
using Firebase.Core;

namespace Plugin.FirebaseStorage
{
    public class FirebaseStorageImplementation : IFirebaseStorage
    {
        public IStorage Storage
        {
            get
            {
                Firebase.Storage.Storage storage;
                if (string.IsNullOrEmpty(FirebaseStorage.DefaultAppName))
                {
                    storage = Firebase.Storage.Storage.DefaultInstance;
                }
                else
                {
                    var app = App.From(FirebaseStorage.DefaultAppName);
                    storage = Firebase.Storage.Storage.From(app);
                }
                return new StorageWrapper(storage);
            }
        }

        public IStorage GetStorage(string appName)
        {
            var app = App.From(appName);
            return new StorageWrapper(Firebase.Storage.Storage.From(app));
        }

        public IStorage GetStorageFromUrl(string url)
        {
            Firebase.Storage.Storage storage;
            if (string.IsNullOrEmpty(FirebaseStorage.DefaultAppName))
            {
                storage = Firebase.Storage.Storage.From(url);
            }
            else
            {
                var app = App.From(FirebaseStorage.DefaultAppName);
                storage = Firebase.Storage.Storage.From(app, url);
            }
            return new StorageWrapper(storage);
        }

        public IStorage GetStorage(string appName, string url)
        {
            var app = App.From(appName);
            return new StorageWrapper(Firebase.Storage.Storage.From(app, url));
        }
    }
}
