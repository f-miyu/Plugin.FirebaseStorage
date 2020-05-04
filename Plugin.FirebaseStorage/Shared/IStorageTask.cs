using System;
namespace Plugin.FirebaseStorage
{
    public interface IStorageTask
    {
        bool IsPaused { get; }
        bool IsInProgress { get; }
        void Cancel();
        void Puase();
        void Resume();
    }
}
