using System;
using System.Collections.Generic;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageMetadataWrapper : IStorageMetadata
    {
        internal StorageMetadata StorageMetadata { get; }

        public string Bucket => StorageMetadata.Bucket;

        public string Generation => StorageMetadata.Generation.ToString();

        public string MetadataGeneration => StorageMetadata.Metageneration.ToString();

        public string Md5Hash => StorageMetadata.Md5Hash;

        public string Path => StorageMetadata.Path;

        public string Name => StorageMetadata.Name;

        public long SizeBytes => StorageMetadata.Size;

        public DateTimeOffset? CreationTime => StorageMetadata.TimeCreated != null ? (DateTimeOffset?)new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(StorageMetadata.TimeCreated.SecondsSinceReferenceDate) : null;

        public DateTimeOffset? UpdatedTime => StorageMetadata.Updated != null ? (DateTimeOffset?)new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(StorageMetadata.Updated.SecondsSinceReferenceDate) : null;

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
                if (StorageMetadata.CustomMetadata != null)
                {
                    foreach (var (key, value) in StorageMetadata.CustomMetadata)
                    {
                        customMetadata.Add(key.ToString(), value.ToString());
                    }
                }
                return customMetadata;
            }
        }

        public IStorageReference Reference => StorageMetadata.StorageReference != null ? new StorageReferenceWrapper(StorageMetadata.StorageReference) : null;

        public StorageMetadataWrapper(StorageMetadata storageMetadata)
        {
            StorageMetadata = storageMetadata;
        }
    }
}
