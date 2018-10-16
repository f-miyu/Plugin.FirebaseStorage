using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Plugin.FirebaseStorage
{
    public interface IStorageReference
    {
        string FullPath { get; }
        IStorageReference GetChild(string path);
        Task PutStreamAsync(Stream stream, MetadataChange metadata = null, IProgress<IUploadTaskSnapshot> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken));
        Task PutBytesAsync(byte[] bytes, MetadataChange metadata = null, IProgress<IUploadTaskSnapshot> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken));
        Task PutFileAsync(string filePath, MetadataChange metadata = null, IProgress<IUploadTaskSnapshot> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken));
        Task<Stream> GetStreamAsync(IProgress<IDownloadTaskSnapshot> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken));
        Task<byte[]> GetBytesAsync(long maxDownloadSizeBytes, IProgress<IDownloadTaskSnapshot> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken));
        Task GetFileAsync(string filePath, IProgress<IDownloadTaskSnapshot> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken));
        Task<Uri> GetDownloadUrlAsync();
    }
}
