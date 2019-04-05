using System;
using System.Collections.Generic;
namespace Plugin.FirebaseStorage
{
    public interface IStorageMetadata
    {
        string Bucket { get; }
        string Generation { get; }
        string MetadataGeneration { get; }
        string Md5Hash { get; }
        string Path { get; }
        string Name { get; }
        long SizeBytes { get; }
        DateTimeOffset CreationTime { get; }
        DateTimeOffset UpdatedTime { get; }
        string CacheControl { get; }
        string ContentDisposition { get; }
        string ContentEncoding { get; }
        string ContentLanguage { get; }
        string ContentType { get; }
        IDictionary<string, string> CustomMetadata { get; }
        IStorageReference Reference { get; }
    }
}
