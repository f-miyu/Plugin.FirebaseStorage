using System;
using Firebase.Core;

namespace Plugin.FirebaseStorage
{
    public class FirebaseStorageImplementation : IFirebaseStorage
    {
        public IStorage Instance => new StorageWrapper(Firebase.Storage.Storage.DefaultInstance);

        public IStorage GetInstance(string appName)
        {
            var app = App.From(appName);
            return new StorageWrapper(Firebase.Storage.Storage.From(app));
        }

        public IStorage GetInstanceFromUrl(string url)
        {
            return new StorageWrapper(Firebase.Storage.Storage.From(url));
        }

        public IStorage GetInstance(string appName, string url)
        {
            var app = App.From(appName);
            return new StorageWrapper(Firebase.Storage.Storage.From(app, url));
        }
    }
}
