using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reactive.Bindings;
using Plugin.Media;
using Xamarin.Forms;
using System.IO;

namespace Plugin.FirebaseStorage.Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public AsyncReactiveCommand SelectImageCommand { get; } = new AsyncReactiveCommand();
        public ReactivePropertySlim<ImageSource> Image { get; } = new ReactivePropertySlim<ImageSource>();
        public ReactivePropertySlim<double> UploadProgress { get; } = new ReactivePropertySlim<double>();
        public ReactivePropertySlim<double> DownloadProgress { get; } = new ReactivePropertySlim<double>();
        public ReactivePropertySlim<string> Name { get; } = new ReactivePropertySlim<string>();
        public ReactivePropertySlim<long> Size { get; } = new ReactivePropertySlim<long>();
        public ReactivePropertySlim<DateTime?> CreationTime { get; } = new ReactivePropertySlim<DateTime?>();
        public ReactivePropertySlim<string> Url { get; } = new ReactivePropertySlim<string>();

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            SelectImageCommand.Subscribe(async () =>
            {
                try
                {
                    if (!CrossMedia.Current.IsPickPhotoSupported)
                    {
                        return;
                    }

                    var file = await CrossMedia.Current.PickPhotoAsync(new Media.Abstractions.PickMediaOptions
                    {
                        PhotoSize = Media.Abstractions.PhotoSize.Medium
                    });

                    if (file == null)
                        return;

                    var name = Path.GetFileName(file.Path);
                    var stream = file.GetStream();

                    file.Dispose();

                    Image.Value = ImageSource.FromStream(() => null);
                    UploadProgress.Value = 0;
                    DownloadProgress.Value = 0;
                    Name.Value = null;
                    Size.Value = 0;
                    CreationTime.Value = null;
                    Url.Value = null;

                    var reference = CrossFirebaseStorage.Current.Instance.RootReference.GetChild(name);

                    var uploadProgress = new Progress<IUploadState>();
                    uploadProgress.ProgressChanged += (sender, e) =>
                    {
                        UploadProgress.Value = e.TotalByteCount > 0 ? 100.0 * e.BytesTransferred / e.TotalByteCount : 0;
                    };

                    var metadataChange = new MetadataChange
                    {
                        ContentType = "image/jpeg"
                    };

                    await reference.PutStreamAsync(stream, metadataChange, uploadProgress);
                    UploadProgress.Value = 100;

                    var downloadProgress = new Progress<IDownloadState>();
                    downloadProgress.ProgressChanged += (sender, e) =>
                    {
                        DownloadProgress.Value = e.TotalByteCount > 0 ? 100.0 * e.BytesTransferred / e.TotalByteCount : 0;
                    };

                    var data = await reference.GetStreamAsync(downloadProgress);
                    DownloadProgress.Value = 100;

                    Image.Value = ImageSource.FromStream(() => data);

                    var metadata = await reference.GetMetadataAsync();

                    Name.Value = metadata.Name;
                    Size.Value = metadata.SizeBytes;
                    CreationTime.Value = metadata.CreationTime.LocalDateTime;

                    var url = await reference.GetDownloadUrlAsync();
                    Url.Value = url.ToString();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            });
        }
    }
}
