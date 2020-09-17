using System;
using Firebase.Storage;
using Foundation;
using System.Linq;
namespace Plugin.FirebaseStorage
{
    internal static class MetadataChangeExtensions
    {
        public static StorageMetadata? ToStorageMetadata(this MetadataChange self)
        {
            if (self == null) return null;

            var storageMetadata = new StorageMetadata();

            if (self.IsCacheControlChanged)
            {
                storageMetadata.CacheControl = self.CacheControl;
            }

            if (self.IsContentDispositionChanged)
            {
                storageMetadata.ContentDisposition = self.ContentDisposition;
            }

            if (self.IsContentEncodingChanged)
            {
                storageMetadata.ContentEncoding = self.ContentEncoding;
            }

            if (self.IsContentLanguageChanged)
            {
                storageMetadata.ContentLanguage = self.ContentLanguage;
            }

            if (self.IsContentTypeChanged)
            {
                storageMetadata.ContentType = self.ContentType;
            }

            if (self.CustomMetadata != null)
            {
                storageMetadata.CustomMetadata = new NSDictionary<NSString, NSString>(self.CustomMetadata.Keys.Select(s => new NSString(s)).ToArray(),
                                                                                      self.CustomMetadata.Values.Select(s => new NSString(s)).ToArray());
            }

            return storageMetadata;
        }
    }
}
