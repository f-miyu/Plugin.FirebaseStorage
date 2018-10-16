using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Storage;
using Android.Runtime;
namespace Plugin.FirebaseStorage
{
    public class StorageReferenceWrapper : IStorageReference
    {
        internal StorageReference StorageReference { get; }

        public string FullPath => throw new NotImplementedException();

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

            UploadTask uploadTask;

            if (metadata != null)
            {
                uploadTask = StorageReference.PutStream(stream, metadata.ToStorageMetadata());
            }
            else
            {
                uploadTask = StorageReference.PutStream(stream);
            }

            return Upload(uploadTask, progress, cancellationToken, pauseToken);
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

        public Task<Stream> GetStreamAsync(IProgress<IDownloadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken))
        {
            var tcs = new TaskCompletionSource<Stream>();

            var downloadTask = StorageReference.GetStream(null);

            downloadTask.AddOnCompleteListener(new OnCompleteHandlerListener(task =>
            {
                if (task.IsSuccessful)
                {
                    var stream = task.Result;
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

        public Task<byte[]> GetBytesAsync(long maxDownloadSizeBytes, IProgress<IDownloadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken))
        {
            throw new NotImplementedException();
        }

        public Task GetFileAsync(string filePath, IProgress<IDownloadState> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken))
        {
            throw new NotImplementedException();
        }

        public Task<Uri> GetDownloadUrlAsync()
        {
            throw new NotImplementedException();
        }
    }
}
