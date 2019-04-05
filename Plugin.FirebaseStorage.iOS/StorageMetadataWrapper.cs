using System;
using System.Collections.Generic;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageMetadataWrapper : IStorageMetadata
    {
        private readonly StorageMetadata _storageMetadata;

        public string Bucket => _storageMetadata.Bucket;

        public string Generation => _storageMetadata.Generation.ToString();

        public string MetadataGeneration => _storageMetadata.Metageneration.ToString();

        public string Md5Hash => _storageMetadata.Md5Hash;

        public string Path => _storageMetadata.Path;

        public string Name => _storageMetadata.Name;

        public long SizeBytes => _storageMetadata.Size;

        public DateTimeOffset CreationTime => _storageMetadata.TimeCreated != null ? new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_storageMetadata.TimeCreated.SecondsSinceReferenceDate) : default(DateTimeOffset);

        public DateTimeOffset UpdatedTime => _storageMetadata.Updated != null ? new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_storageMetadata.Updated.SecondsSinceReferenceDate) : default(DateTimeOffset);

        public string CacheControl => _storageMetadata.CacheControl;

        public string ContentDisposition => _storageMetadata.ContentDisposition;

        public string ContentEncoding => _storageMetadata.ContentEncoding;

        public string ContentLanguage => _storageMetadata.ContentLanguage;

        public string ContentType => _storageMetadata.ContentType;

        public IDictionary<string, string> CustomMetadata
        {
            get
            {
                var customMetadata = new Dictionary<string, string>();
                if (_storageMetadata.CustomMetadata != null)
                {
                    foreach (var (key, value) in _storageMetadata.CustomMetadata)
                    {
                        customMetadata.Add(key.ToString(), value.ToString());
                    }
                }
                return customMetadata;
            }
        }

        public IStorageReference Reference => _storageMetadata.StorageReference != null ? new StorageReferenceWrapper(_storageMetadata.StorageReference) : null;

        public StorageMetadataWrapper(StorageMetadata storageMetadata)
        {
            _storageMetadata = storageMetadata;
        }
    }
}
