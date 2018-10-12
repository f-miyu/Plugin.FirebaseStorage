using System;
namespace Plugin.FirebaseStorage
{
    public class StorageWrapper : IStorage
    {
        internal Firebase.Storage.FirebaseStorage Storage { get; }

        public StorageWrapper(Firebase.Storage.FirebaseStorage storage)
        {
            Storage = storage;
        }

        public IStorageReference GetReferenceFromPath(string path)
        {
            var reference = Storage.GetReference(path);
            return new StorageReferenceWrapper(reference);
        }

        public IStorageReference GetReferenceFromUrl(string url)
        {
            var reference = Storage.GetReferenceFromUrl(url);
            return new StorageReferenceWrapper(reference);
        }

        public IStorageReference GetRootReference()
        {
            var reference = Storage.Reference;
            return new StorageReferenceWrapper(reference);
        }
    }
}
