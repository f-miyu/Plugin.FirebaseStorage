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
        Task PutStreamAsync(Stream stream, IProgress<IUploadTaskSnapshot> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken));
        Task PutBytesAsync(byte[] bytes, IProgress<IUploadTaskSnapshot> progress = null, CancellationToken cancellationToken = default(CancellationToken), PauseToken pauseToken = default(PauseToken));
    }
}
