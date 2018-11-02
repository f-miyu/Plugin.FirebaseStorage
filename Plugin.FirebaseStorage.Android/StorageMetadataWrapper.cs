using System;
using System.Collections.Generic;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class StorageMetadataWrapper : IStorageMetadata
    {
        private readonly StorageMetadata _storageMetadata;

        public string Bucket => _storageMetadata.Bucket;

        public string Generation => _storageMetadata.Generation;

        public string MetadataGeneration => _storageMetadata.MetadataGeneration;

        public string Md5Hash => _storageMetadata.Md5Hash;

        public string Path => _storageMetadata.Path;

        public string Name => _storageMetadata.Name;

        public long SizeBytes => _storageMetadata.SizeBytes;

        public DateTimeOffset? CreationTime => new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(_storageMetadata.CreationTimeMillis);

        public DateTimeOffset? UpdatedTime => new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddMilliseconds(_storageMetadata.UpdatedTimeMillis);

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
                if (_storageMetadata.CustomMetadataKeys != null)
                {
                    foreach (var key in _storageMetadata.CustomMetadataKeys)
                    {
                        customMetadata.Add(key, _storageMetadata.GetCustomMetadata(key));
                    }
                }
                return customMetadata;
            }
        }

        public IStorageReference Reference => _storageMetadata.Reference != null ? new StorageReferenceWrapper(_storageMetadata.Reference) : null;

        public StorageMetadataWrapper(StorageMetadata storageMetadata)
        {
            _storageMetadata = storageMetadata;
        }
    }
}
