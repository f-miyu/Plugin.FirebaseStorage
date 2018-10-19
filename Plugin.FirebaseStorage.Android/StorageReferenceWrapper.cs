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
        internal StorageReference StorageReference { get; }

        public string Name => StorageReference.Name;

        public string Path => StorageReference.Path;

        public string Bucket => StorageReference.Bucket;

        public IStorageReference Parent => new StorageReferenceWrapper(StorageReference.Parent);

        public IStorageReference Root => new StorageReferenceWrapper(StorageReference.Root);

        public IStorage Storage => new StorageWrapper(StorageReference.Storage);

        public StorageReferenceWrapper(StorageReference storageReference)
        {
            StorageReference = storageReference;
        }

        public IStorageReference GetChild(string path)
        {
            var reference = StorageReference.Child(path);
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
                uploadTask = StorageReference.PutBytes(bytes, metadata.ToStorageMetadata());
            }
            else
            {
                uploadTask = StorageReference.PutBytes(bytes);
            }

            return Upload(uploadTask, progress, cancellationToken, pauseToken);
        }

        public Task PutFileAsync(string filePath, MetadataChange metadata = null, IProgress<IUploadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken))
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            var uri = Android.Net.Uri.Parse(filePath);

            UploadTask uploadTask;

            if (metadata != null)
            {
                uploadTask = StorageReference.PutFile(uri, metadata.ToStorageMetadata());
            }
            else
            {
                uploadTask = StorageReference.PutFile(uri);
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

            var downloadTask = StorageReference.GetStream(new StreamProcessor(tcs, maxDownloadSizeBytes));

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

        public Task GetFileAsync(string filePath, IProgress<IDownloadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken))
        {
            var tcs = new TaskCompletionSource<byte[]>();

            var downloadTask = StorageReference.GetFile(Android.Net.Uri.Parse(filePath));

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

            if (pauseToken != default(PauseToken))
            {
                pauseToken.SetStorageTask(new StorageTaskWrapper(downloadTask));
            }

            return tcs.Task;
        }

        public Task<Uri> GetDownloadUrlAsync()
        {
            var tcs = new TaskCompletionSource<Uri>();

            StorageReference.DownloadUrl.AddOnCompleteListener(new OnCompleteHandlerListener(task =>
            {
                if (task.IsSuccessful)
                {
                    var uri = task.Result.JavaCast<Android.Net.Uri>();
                    tcs.SetResult(new Uri(uri.ToString()));
                }
                else
                {
                    tcs.SetException(ExceptionMapper.Map(task.Exception));
                }
            }));

            return tcs.Task;
        }

        public async Task DeleteAsync()
        {
            try
            {
                await StorageReference.DeleteAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task<IStorageMetadata> GetMetadataAsync()
        {
            var tcs = new TaskCompletionSource<IStorageMetadata>();

            StorageReference.Metadata.AddOnCompleteListener(new OnCompleteHandlerListener(task =>
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

            StorageReference.UpdateMetadata(metadata.ToStorageMetadata()).AddOnCompleteListener(new OnCompleteHandlerListener(task =>
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
                try
                {
                    var buffer = new ByteArrayOutputStream();

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
                }
            }
        }
    }
}
