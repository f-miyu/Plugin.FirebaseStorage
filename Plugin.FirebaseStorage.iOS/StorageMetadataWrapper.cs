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

        public string CacheControl => StorageMetadata.CacheControl;

        public string ContentDisposition => StorageMetadata.ContentDisposition;

        public string ContentEncoding => StorageMetadata.ContentEncoding;

        public string ContentLanguage => StorageMetadata.ContentLanguage;

        public string ContentType => StorageMetadata.ContentType;

        public IDictionary<string, string> CustomMetadata
        {
            get
            {
                var customMetadata = new Dictionary<string, string>();
                foreach (var (key, value) in StorageMetadata.CustomMetadata)
                {
                    customMetadata.Add(key.ToString(), value.ToString());
                }

                return customMetadata;
            }
        }

        public StorageMetadataWrapper(StorageMetadata storageMetadata)
        {
            StorageMetadata = storageMetadata;
        }
    }
}
