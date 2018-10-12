using System;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class FirebaseStorageImplementation : IFirebaseStorage
    {
        private IStorage _storage;
        public IStorage Storage => _storage ?? (_storage = new StorageWrapper(Firebase.Storage.Storage.DefaultInstance));

        public IStorage GetStorage(string url)
        {
            return new StorageWrapper(Firebase.Storage.Storage.From(url));
        }
    }
}
