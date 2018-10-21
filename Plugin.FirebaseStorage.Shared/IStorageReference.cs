using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.FirebaseStorage
{
    public interface IStorageReference
    {
        string Name { get; }
        string Path { get; }
        string Bucket { get; }
        IStorageReference Parent { get; }
        IStorageReference Root { get; }
        IStorage Storage { get; }
        IStorageReference GetChild(string path);
        Task PutStreamAsync(Stream stream, MetadataChange metadata = null, IProgress<IUploadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken));
        Task PutBytesAsync(byte[] bytes, MetadataChange metadata = null, IProgress<IUploadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken));
        Task PutFileAsync(string filePath, MetadataChange metadata = null, IProgress<IUploadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken));
        Task<Stream> GetStreamAsync(IProgress<IDownloadState> progress = null, CancellationToken cancellationToken = default(CancellationToken));
        Task<byte[]> GetBytesAsync(long maxDownloadSizeBytes, IProgress<IDownloadState> progress = null, CancellationToken cancellationToken = default(CancellationToken));
        Task GetFileAsync(string filePath, IProgress<IDownloadState> progress = null, CancellationToken cancellationToken = default(CancellationToken));
        Task<Uri> GetDownloadUrlAsync();
        Task DeleteAsync();
        Task<IStorageMetadata> GetMetadataAsync();
        Task<IStorageMetadata> UpdateMetadataAsync(MetadataChange metadata);
    }
}
