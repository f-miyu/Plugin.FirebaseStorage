using System;
using System.Collections.Generic;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageMetadataWrapper : IStorageMetadata
    {
        internal StorageMetadata StorageMetadata { get; }

        public string Name => StorageMetadata.Name;

        public string Path => StorageMetadata.Path;

        public long Size => StorageMetadata.Size;

        public string CacheControl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentDisposition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentEncoding { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentLanguage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Dictionary<string, string> CustomMetadata { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        internal StorageMetadataWrapper(StorageMetadata storageMetadata)
        {
            StorageMetadata = storageMetadata;
        }

        public StorageMetadataWrapper()
        {

        }
    }
}
