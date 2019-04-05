using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Storage;
using Android.Runtime;
using Java.IO;
using Android.Gms.Common.Apis;

namespace Plugin.FirebaseStorage
{
    public class StorageReferenceWrapper : IStorageReference
    {
        private readonly StorageReference _storageReference;

        public string Name => _storageReference.Name;

        public string Path => _storageReference.Path;

        public string Bucket => _storageReference.Bucket;

        public IStorageReference Parent
        {
            get
            {
                var parent = _storageReference.Parent;
                return parent != null ? new StorageReferenceWrapper(parent) : null;
            }
        }

        public IStorageReference Root
        {
            get
            {
                var root = _storageReference.Root;
                return root != null ? new StorageReferenceWrapper(root) : null;
            }
        }

        public IStorage Storage => _storageReference.Storage != null ? new StorageWrapper(_storageReference.Storage) : null;

        public StorageReferenceWrapper(StorageReference storageReference)
        {
            _storageReference = storageReference;
        }

        public IStorageReference GetChild(string path)
        {
            var reference = _storageReference.Child(path);
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

            UploadTask uploadTask;

            if (metadata != null)
            {
                uploadTask = _storageReference.PutBytes(bytes, metadata.ToStorageMetadata());
            }
            else
            {
                uploadTask = _storageReference.PutBytes(bytes);
            }

            return Upload(uploadTask, progress, cancellationToken, pauseToken);
        }

        public Task PutFileAsync(string filePath, MetadataChange metadata = null, IProgress<IUploadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken))
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var uri = Android.Net.Uri.FromFile(new Java.IO.File(filePath));

            UploadTask uploadTask;

            if (metadata != null)
            {
                uploadTask = _storageReference.PutFile(uri, metadata.ToStorageMetadata());
            }
            else
            {
                uploadTask = _storageReference.PutFile(uri);
            }

            return Upload(uploadTask, progress, cancellationToken, pauseToken);
        }

        private Task Upload(UploadTask uploadTask, IProgress<IUploadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken))
        {
            var tcs = new TaskCompletionSource<bool>();

            uploadTask.AddOnCompleteListener(new OnCompleteHandlerListener(task =>
            {
                if (task.IsSuccessful)
                {
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            if (progress != null)
            {
                uploadTask.AddOnProgressListener(new OnProgressHandlerListener(snapshot =>
                {
                    var uploadTaskSnapshot = snapshot.JavaCast<UploadTask.TaskSnapshot>();
                    progress.Report(new UploadTaskSnapshotWrapper(uploadTaskSnapshot));
                }));
            }

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(() => uploadTask.Cancel());
            }

            if (pauseToken != default(PauseToken))
            {
                pauseToken.SetStorageTask(new StorageTaskWrapper(uploadTask));
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

            var downloadTask = _storageReference.GetStream(new StreamProcessor(tcs, maxDownloadSizeBytes));

            downloadTask.AddOnCompleteListener(new OnCompleteHandlerListener(task =>
            {
                if (task.IsSuccessful)
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        var exception = StorageException.FromErrorStatus(new Statuses(CommonStatusCodes.InternalError));
                        tcs.TrySetException(ExceptionMapper.Map(exception));
                    }
                }
                else
                {
                    tcs.TrySetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            if (progress != null)
            {
                downloadTask.AddOnProgressListener(new OnProgressHandlerListener(snapshot =>
                {
                    var downloadTaskSnapshot = snapshot.JavaCast<StreamDownloadTask.TaskSnapshot>();
                    progress.Report(new StreamDownloadTaskSnapshotWrapper(downloadTaskSnapshot));
                }));
            }

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(() => downloadTask.Cancel());
            }

            return tcs.Task;
        }

        public Task GetFileAsync(string filePath, IProgress<IDownloadState> progress = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<bool>();

            var downloadTask = _storageReference.GetFile(Android.Net.Uri.FromFile(new Java.IO.File(filePath)));

            downloadTask.AddOnCompleteListener(new OnCompleteHandlerListener(task =>
            {
                if (task.IsSuccessful)
                {
                    tcs.SetResult(true);
                }
                else
                {
                    tcs.SetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            if (progress != null)
            {
                downloadTask.AddOnProgressListener(new OnProgressHandlerListener(snapshot =>
                {
                    var downloadTaskSnapshot = snapshot.JavaCast<FileDownloadTask.TaskSnapshot>();
                    progress.Report(new FileDownloadTaskSnapshotWrapper(downloadTaskSnapshot));
                }));
            }

            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(() => downloadTask.Cancel());
            }

            return tcs.Task;
        }

        public async Task<Uri> GetDownloadUrlAsync()
        {
            try
            {
                var uri = await _storageReference.GetDownloadUrlAsync().ConfigureAwait(false);
                return new Uri(uri.ToString());
            }
            catch (Exception e)
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
            catch (Exception e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task<IStorageMetadata> GetMetadataAsync()
        {
            var tcs = new TaskCompletionSource<IStorageMetadata>();

            _storageReference.GetMetadata().AddOnCompleteListener(new OnCompleteHandlerListener(task =>
            {
                if (task.IsSuccessful)
                {
                    var result = task.Result.JavaCast<StorageMetadata>();
                    tcs.SetResult(new StorageMetadataWrapper(result));
                }
                else
                {
                    tcs.SetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            return tcs.Task;
        }

        public Task<IStorageMetadata> UpdateMetadataAsync(MetadataChange metadata)
        {
            var tcs = new TaskCompletionSource<IStorageMetadata>();

            _storageReference.UpdateMetadata(metadata.ToStorageMetadata()).AddOnCompleteListener(new OnCompleteHandlerListener(task =>
            {
                if (task.IsSuccessful)
                {
                    var result = task.Result.JavaCast<StorageMetadata>();
                    tcs.SetResult(new StorageMetadataWrapper(result));
                }
                else
                {
                    tcs.SetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            return tcs.Task;
        }

        private class StreamProcessor : Java.Lang.Object, StreamDownloadTask.IStreamProcessor
        {
            private TaskCompletionSource<byte[]> _tcs;
            private long _maxDownloadSizeBytes;

            public StreamProcessor(TaskCompletionSource<byte[]> tcs, long maxDownloadSizeBytes)
            {
                _tcs = tcs;
                _maxDownloadSizeBytes = maxDownloadSizeBytes;
            }

            public void DoInBackground(StreamDownloadTask.TaskSnapshot state, Stream stream)
            {
                var buffer = new ByteArrayOutputStream();
                try
                {
                    var data = new byte[16384];
                    int n = 0;
                    long total = 0;
                    while ((n = stream.Read(data, 0, data.Length)) > 0)
                    {
                        total += n;
                        if (total > _maxDownloadSizeBytes)
                        {
                            throw new FirebaseStorageException("the maximum allowed buffer size was exceeded.", ErrorType.DownloadSizeExceeded);
                        }
                        buffer.Write(data, 0, n);
                    }
                    buffer.Flush();
                    _tcs.TrySetResult(buffer.ToByteArray());
                }
                catch (FirebaseStorageException e)
                {
                    _tcs.TrySetException(e);
                }
                finally
                {
                    stream.Close();
                    buffer.Close();
                }
            }
        }
    }
}
