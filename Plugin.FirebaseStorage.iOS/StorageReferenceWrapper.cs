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

        public string FullPath => StorageReference.FullPath;

        public StorageReferenceWrapper(StorageReference storageReference)
        {
            StorageReference = storageReference;
        }

        public IStorageReference GetChild(string path)
        {
            var reference = StorageReference.GetChild(path);
            return new StorageReferenceWrapper(reference);
        }

        public Task PutStreamAsync(Stream stream, IProgress<IUploadTaskSnapshot> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken))
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return PutBytesAsync(ms.ToArray(), progress, cancellationToken, pauseToken);
            }
        }

        public async Task PutBytesAsync(byte[] bytes, IProgress<IUploadTaskSnapshot> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken))
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            StorageUploadTask storageUploadTask = null;
            string observer = null;
            CancellationTokenRegistration? registration = null;

            try
            {
                var data = NSData.FromArray(bytes);
                var tcs = new TaskCompletionSource<bool>();

                storageUploadTask = StorageReference.PutData(data, null, (metadata, error) =>
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
                    observer = storageUploadTask.ObserveStatus(StorageTaskStatus.Progress, snapshot => progress.Report(new UploadTaskSnapshotWrapper(snapshot)));
                }

                if (cancellationToken != default(CancellationToken))
                {
                    registration = cancellationToken.Register(storageUploadTask.Cancel);
                }

                if (pauseToken != default(PauseToken))
                {
                    pauseToken.StorageTask = new StorageUploadTaskWrapper(storageUploadTask);
                }

                await tcs.Task.ConfigureAwait(false);
            }
            finally
            {
                if (storageUploadTask != null)
                {
                    if (observer != null)
                    {
                        storageUploadTask.RemoveObserver(observer);
                    }

                    if (registration != null)
                    {
                        registration.Value.Dispose();
                    }

                    pauseToken.StorageTask = null;
                }
            }
        }
    }
}
