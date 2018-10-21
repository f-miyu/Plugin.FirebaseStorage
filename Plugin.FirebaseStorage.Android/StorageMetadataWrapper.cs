using System;
using System.Collections.Generic;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class StorageMetadataWrapper : IStorageMetadata
    {
        internal StorageMetadata StorageMetadata { get; }

        public string Bucket => StorageMetadata.Bucket;

        public string Generation => StorageMetadata.Generation;

        public string MetadataGeneration => StorageMetadata.MetadataGeneration;

        public string Md5Hash => StorageMetadata.Md5Hash;

        public string Path => StorageMetadata.Path;

        public string Name => StorageMetadata.Name;

        public long SizeBytes => StorageMetadata.SizeBytes;

        public DateTimeOffset? CreationTime => new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(StorageMetadata.CreationTimeMillis);

        public DateTimeOffset? UpdatedTime => new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(StorageMetadata.UpdatedTimeMillis);

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
                if (StorageMetadata.CustomMetadataKeys != null)
                {
                    foreach (var key in StorageMetadata.CustomMetadataKeys)
                    {
                        customMetadata.Add(key, StorageMetadata.GetCustomMetadata(key));
                    }
                }
                return customMetadata;
            }
        }

        public IStorageReference Reference => StorageMetadata.Reference != null ? new StorageReferenceWrapper(StorageMetadata.Reference) : null;

        public StorageMetadataWrapper(StorageMetadata storageMetadata)
        {
            StorageMetadata = storageMetadata;
        }
    }
}
