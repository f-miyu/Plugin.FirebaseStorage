using System;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class StorageWrapper : IStorage
    {
        private readonly Storage _storage;

        public IStorageReference RootReference => new StorageReferenceWrapper(_storage.GetRootReference());

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
