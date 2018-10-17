using System;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class StorageWrapper : IStorage
    {
        internal Storage Storage { get; }

        public IStorageReference RootReference => new StorageReferenceWrapper(Storage.GetRootReference());

        public StorageWrapper(Storage storage)
        {
            Storage = storage;
        }

        public IStorageReference GetReferenceFromPath(string path)
        {
            var reference = Storage.GetReferenceFromPath(path);
            return new StorageReferenceWrapper(reference);
        }

        public IStorageReference GetReferenceFromUrl(string url)
        {
            var reference = Storage.GetReferenceFromUrl(url);
            return new StorageReferenceWrapper(reference);
        }
    }
}
