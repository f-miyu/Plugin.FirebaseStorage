using System;
using System.ComponentModel;
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
        IStorageReference? Parent { get; }
        IStorageReference Root { get; }
        IStorage Storage { get; }
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Please use Child(string path) method instead.")]
        IStorageReference GetChild(string path);
        IStorageReference Child(string path);
        Task PutStreamAsync(Stream stream, MetadataChange? metadata = null, IProgress<IUploadState>? progress = null, CancellationToken cancellationToken = default, PauseToken pauseToken = default);
        Task PutBytesAsync(byte[] bytes, MetadataChange? metadata = null, IProgress<IUploadState>? progress = null, CancellationToken cancellationToken = default, PauseToken pauseToken = default);
        Task PutFileAsync(string filePath, MetadataChange? metadata = null, IProgress<IUploadState>? progress = null, CancellationToken cancellationToken = default, PauseToken pauseToken = default);
        Task<Stream> GetStreamAsync(IProgress<IDownloadState>? progress = null, CancellationToken cancellationToken = default);
        Task<byte[]> GetBytesAsync(long maxDownloadSizeBytes, IProgress<IDownloadState>? progress = null, CancellationToken cancellationToken = default);
        Task GetFileAsync(string filePath, IProgress<IDownloadState>? progress = null, CancellationToken cancellationToken = default);
        Task<Uri> GetDownloadUrlAsync();
        Task DeleteAsync();
        Task<IStorageMetadata> GetMetadataAsync();
        Task<IStorageMetadata> UpdateMetadataAsync(MetadataChange metadata);
        Task<IListResult> List(int maxResults);
        Task<IListResult> List(int maxResults, string pageToken);
        Task<IListResult> ListAll();
    }
}
