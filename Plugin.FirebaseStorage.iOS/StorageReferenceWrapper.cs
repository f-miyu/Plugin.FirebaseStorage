using System;
using Firebase.Storage;
using System.Threading.Tasks;
using System.IO;
using Foundation;
using System.Threading;
namespace Plugin.FirebaseStorage
{
    public class StorageReferenceWrapper : IStorageReference
    {
        internal StorageReference StorageReference { get; }

        public string Name => StorageReference.Name;

        public string Path => StorageReference.FullPath;

        public string Bucket => StorageReference.Bucket;

        public IStorageReference Parent
        {
            get
            {
                var parent = StorageReference.Parent;
                return parent != null ? new StorageReferenceWrapper(parent) : null;
            }
        }

        public IStorageReference Root
        {
            get
            {
                var root = StorageReference.Root;
                return root != null ? new StorageReferenceWrapper(root) : null;
            }
        }

        public IStorage Storage => StorageReference.Storage != null ? new StorageWrapper(StorageReference.Storage) : null;

        public StorageReferenceWrapper(StorageReference storageReference)
        {
            StorageReference = storageReference;
        }

        public IStorageReference GetChild(string path)
        {
            var reference = StorageReference.GetChild(path);
            return new StorageReferenceWrapper(reference);
        }

        public Task PutStreamAsync(Stream stream, MetadataChange metadata = null, IProgress<IUploadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken))
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return PutBytesAsync(ms.ToArray(), metadata, progress, cancellationToken, pauseToken);
            }
        }

        public Task PutBytesAsync(byte[] bytes, MetadataChange metadata = null, IProgress<IUploadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken))
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            var data = NSData.FromArray(bytes);
            var tcs = new TaskCompletionSource<bool>();

            var uploadTask = StorageReference.PutData(data, metadata?.ToStorageMetadata(), (storageMetadata, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(new NSErrorException(error)));
                }
                else
                {
                    tcs.SetResult(true);
                }
            });

            if (progress != null)
            {
                uploadTask.ObserveStatus(StorageTaskStatus.Progress, snapshot => progress.Report(new StorageTaskSnapshotWrapper(snapshot)));
            }

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(uploadTask.Cancel);
            }

            if (pauseToken != default(PauseToken))
            {
                pauseToken.SetStorageTask(new StorageUploadTaskWrapper(uploadTask));
            }

            return tcs.Task;
        }

        public Task PutFileAsync(string filePath, MetadataChange metadata = null, IProgress<IUploadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken))
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var tcs = new TaskCompletionSource<bool>();

            var uploadTask = StorageReference.PutFile(NSUrl.FromFilename(filePath), metadata?.ToStorageMetadata(), (storageMetadata, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(new NSErrorException(error)));
                }
                else
                {
                    tcs.SetResult(true);
                }
            });

            if (progress != null)
            {
                uploadTask.ObserveStatus(StorageTaskStatus.Progress, snapshot => progress.Report(new StorageTaskSnapshotWrapper(snapshot)));
            }

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(uploadTask.Cancel);
            }

            if (pauseToken != default(PauseToken))
            {
                pauseToken.SetStorageTask(new StorageUploadTaskWrapper(uploadTask));
            }

            return tcs.Task;
        }

        public async Task<Stream> GetStreamAsync(IProgress<IDownloadState> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var data = await GetBytesAsync(long.MaxValue, progress, cancellationToken).ConfigureAwait(false);
            return new MemoryStream(data);
        }

        public Task<byte[]> GetBytesAsync(long maxDownloadSizeBytes, IProgress<IDownloadState> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<byte[]>();

            var downloadTask = StorageReference.GetData(maxDownloadSizeBytes, (data, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(new NSErrorException(error)));
                }
                else
                {
                    tcs.SetResult(data.ToArray());
                }
            });

            if (progress != null)
            {
                downloadTask.ObserveStatus(StorageTaskStatus.Progress, snapshot => progress.Report(new StorageTaskSnapshotWrapper(snapshot)));
            }

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(downloadTask.Cancel);
            }

            return tcs.Task;
        }

        public Task GetFileAsync(string filePath, IProgress<IDownloadState> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var url = NSUrl.FromFilename(filePath);

            var tcs = new TaskCompletionSource<bool>();

            var downloadTask = StorageReference.WriteToFile(url, (data, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(new NSErrorException(error)));
                }
                else
                {
                    tcs.SetResult(true);
                }
            });

            if (progress != null)
            {
                downloadTask.ObserveStatus(StorageTaskStatus.Progress, snapshot => progress.Report(new StorageTaskSnapshotWrapper(snapshot)));
            }

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(downloadTask.Cancel);
            }

            return tcs.Task;
        }

        public async Task<Uri> GetDownloadUrlAsync()
        {
            try
            {
                var url = await StorageReference.GetDownloadUrlAsync().ConfigureAwait(false);
                return new Uri(url.AbsoluteString);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task DeleteAsync()
        {
            try
            {
                await StorageReference.DeleteAsync().ConfigureAwait(false);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IStorageMetadata> GetMetadataAsync()
        {
            try
            {
                var metadata = await StorageReference.GetMetadataAsync().ConfigureAwait(false);
                return new StorageMetadataWrapper(metadata);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public async Task<IStorageMetadata> UpdateMetadataAsync(MetadataChange metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException(nameof(metadata));

            try
            {
                var result = await StorageReference.UpdateMetadataAsync(metadata.ToStorageMetadata()).ConfigureAwait(false);
                return new StorageMetadataWrapper(result);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }
    }
}