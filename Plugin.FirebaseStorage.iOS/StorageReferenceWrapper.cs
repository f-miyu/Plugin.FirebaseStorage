using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageReferenceWrapper : IStorageReference
    {
        internal StorageReference StorageReference { get; }

        public string Name => StorageReference.Name;

        public string FullPath => StorageReference.FullPath;

        public StorageReferenceWrapper(StorageReference storageReference)
        {
            StorageReference = storageReference;
        }

        public IStorageReference GetChild(string path)
        {
            var reference = StorageReference.GetChild(path);
            return new StorageReferenceWrapper(reference);
        }
    }
}
