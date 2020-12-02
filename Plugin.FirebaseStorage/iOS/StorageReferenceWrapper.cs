using System;
using Firebase.Storage;
using System.Threading.Tasks;
using System.IO;
using Foundation;
using System.Threading;
namespace Plugin.FirebaseStorage
{
    public class StorageReferenceWrapper : IStorageReference, IEquatable<StorageReferenceWrapper>
    {
        private readonly StorageReference _storageReference;

        public StorageReferenceWrapper(StorageReference storageReference)
        {
            _storageReference = storageReference ?? throw new ArgumentNullException(nameof(storageReference));
        }

        public string Name => _storageReference.Name;

        public string Path => _storageReference.FullPath;

        public string Bucket => _storageReference.Bucket;

        public IStorageReference? Parent
        {
            get
            {
                var parent = _storageReference.Parent;
                return parent != null ? new StorageReferenceWrapper(parent) : null;
            }
        }

        public IStorageReference Root => new StorageReferenceWrapper(_storageReference.Root);

        public IStorage Storage => StorageProvider.GetStorage(_storageReference.Storage);

        public IStorageReference GetChild(string path)
        {
            return Child(path);
        }

        public IStorageReference Child(string path)
        {
            var reference = _storageReference.GetChild(path);
            return new StorageReferenceWrapper(reference);
        }

        public Task PutStreamAsync(Stream stream, MetadataChange? metadata = null, IProgress<IUploadState>? progress = null, CancellationToken cancellationToken = default, PauseToken pauseToken = default)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return PutBytesAsync(ms.ToArray(), metadata, progress, cancellationToken, pauseToken);
            }
        }

        public Task PutBytesAsync(byte[] bytes, MetadataChange? metadata = null, IProgress<IUploadState>? progress = null, CancellationToken cancellationToken = default, PauseToken pauseToken = default)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            var data = NSData.FromArray(bytes);
            var tcs = new TaskCompletionSource<bool>();

            var uploadTask = _storageReference.PutData(data, metadata?.ToStorageMetadata(), (storageMetadata, error) =>
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

            if (cancellationToken != default)
            {
                cancellationToken.Register(uploadTask.Cancel);
            }

            if (pauseToken != default)
            {
                pauseToken.SetStorageTask(new StorageUploadTaskWrapper(uploadTask));
            }

            return tcs.Task;
        }

        public Task PutFileAsync(string filePath, MetadataChange? metadata = null, IProgress<IUploadState>? progress = null, CancellationToken cancellationToken = default, PauseToken pauseToken = default)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var tcs = new TaskCompletionSource<bool>();

            var uploadTask = _storageReference.PutFile(NSUrl.FromFilename(filePath), metadata?.ToStorageMetadata(), (storageMetadata, error) =>
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

            if (cancellationToken != default)
            {
                cancellationToken.Register(uploadTask.Cancel);
            }

            if (pauseToken != default)
            {
                pauseToken.SetStorageTask(new StorageUploadTaskWrapper(uploadTask));
            }

            return tcs.Task;
        }

        public async Task<Stream> GetStreamAsync(IProgress<IDownloadState>? progress = null, CancellationToken cancellationToken = default)
        {
            var data = await GetBytesAsync(long.MaxValue, progress, cancellationToken).ConfigureAwait(false);
            return new MemoryStream(data);
        }

        public Task<byte[]> GetBytesAsync(long maxDownloadSizeBytes, IProgress<IDownloadState>? progress = null, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<byte[]>();

            var downloadTask = _storageReference.GetData(maxDownloadSizeBytes, (data, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(new NSErrorException(error)));
                }
                else
                {
                    tcs.SetResult(data!.ToArray());
                }
            });

            if (progress != null)
            {
                downloadTask.ObserveStatus(StorageTaskStatus.Progress, snapshot => progress.Report(new StorageTaskSnapshotWrapper(snapshot)));
            }

            if (cancellationToken != default)
            {
                cancellationToken.Register(downloadTask.Cancel);
            }

            return tcs.Task;
        }

        public Task GetFileAsync(string filePath, IProgress<IDownloadState>? progress = null, CancellationToken cancellationToken = default)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var url = NSUrl.FromFilename(filePath);

            var tcs = new TaskCompletionSource<bool>();

            var downloadTask = _storageReference.WriteToFile(url, (data, error) =>
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

            if (cancellationToken != default)
            {
                cancellationToken.Register(downloadTask.Cancel);
            }

            return tcs.Task;
        }

        public async Task<Uri> GetDownloadUrlAsync()
        {
            try
            {
                var url = await _storageReference.GetDownloadUrlAsync().ConfigureAwait(false);
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
                await _storageReference.DeleteAsync().ConfigureAwait(false);
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
                var metadata = await _storageReference.GetMetadataAsync().ConfigureAwait(false);
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
                var result = await _storageReference.UpdateMetadataAsync(metadata.ToStorageMetadata()!).ConfigureAwait(false);
                return new StorageMetadataWrapper(result);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task<IListResult> ListAsync(int maxResults)
        {
            var tcs = new TaskCompletionSource<IListResult>();

            _storageReference.List(maxResults, (result, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(new NSErrorException(error)));
                }
                else
                {
                    tcs.SetResult(new ListResultWrapper(result));
                }
            });

            return tcs.Task;
        }

        public Task<IListResult> ListAsync(int maxResults, string pageToken)
        {
            var tcs = new TaskCompletionSource<IListResult>();

            _storageReference.List(maxResults, pageToken, (result, error) =>
             {
                 if (error != null)
                 {
                     tcs.SetException(ExceptionMapper.Map(new NSErrorException(error)));
                 }
                 else
                 {
                     tcs.SetResult(new ListResultWrapper(result));
                 }
             });

            return tcs.Task;
        }

        public Task<IListResult> ListAllAsync()
        {
            var tcs = new TaskCompletionSource<IListResult>();

            _storageReference.ListAll((result, error) =>
            {
                if (error != null)
                {
                    tcs.SetException(ExceptionMapper.Map(new NSErrorException(error)));
                }
                else
                {
                    tcs.SetResult(new ListResultWrapper(result));
                }
            });

            return tcs.Task;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as StorageReferenceWrapper);
        }

        public bool Equals(StorageReferenceWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_storageReference, other._storageReference)) return true;
            return _storageReference.Equals(other._storageReference);
        }

        public override int GetHashCode()
        {
            return _storageReference.GetHashCode();
        }
    }
}