using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    internal static class MetadataChangeExtensions
    {
        public static StorageMetadata? ToStorageMetadata(this MetadataChange self)
        {
            if (self == null) return null;

            var builder = new StorageMetadata.Builder();

            if (self.IsCacheControlChanged)
            {
                builder.SetCacheControl(self.CacheControl);
            }

            if (self.IsContentDispositionChanged)
            {
                builder.SetContentDisposition(self.ContentDisposition);
            }

            if (self.IsContentEncodingChanged)
            {
                builder.SetContentEncoding(self.ContentEncoding);
            }

            if (self.IsContentLanguageChanged)
            {
                builder.SetContentLanguage(self.ContentLanguage);
            }

            if (self.IsContentTypeChanged)
            {
                builder.SetContentType(self.ContentType);
            }

            if (self.CustomMetadata != null)
            {
                foreach (var (key, value) in self.CustomMetadata)
                {
                    builder.SetCustomMetadata(key, value);
                }
            }

            return builder.Build();
        }
    }
}
