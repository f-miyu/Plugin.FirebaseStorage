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
using System.Threading.Tasks;
using System.Threading;
using Prism.Services;
using Xamarin.Essentials;

namespace Plugin.FirebaseStorage.Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public AsyncReactiveCommand SelectImageCommand { get; } = new AsyncReactiveCommand();
        public ReactivePropertySlim<ImageSource> Image { get; } = new ReactivePropertySlim<ImageSource>();
        public ReactivePropertySlim<double> UploadProgress { get; } = new ReactivePropertySlim<double>();
        public ReactivePropertySlim<double> DownloadProgress { get; } = new ReactivePropertySlim<double>();

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            SelectImageCommand.Subscribe(async () =>
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

                try
                {
                    var name = Path.GetFileName(file.Path);
                    var stream = file.GetStream();

                    file.Dispose();

                    UploadProgress.Value = 0;
                    DownloadProgress.Value = 0;

                    var reference = CrossFirebaseStorage.Current.Storage.RootReference.GetChild(name);

                    var path = reference.Path;
                    var pts = new PauseTokenSource();

                    var uploadProgress = new Progress<IUploadState>();
                    uploadProgress.ProgressChanged += (sender, e) =>
                    {
                        UploadProgress.Value = e.TotalByteCount > 0 ? 100.0 * e.BytesTransferred / e.TotalByteCount : 0;
                    };

                    var metadata = new MetadataChange
                    {
                        ContentType = "image/jpeg"
                    };

                    await reference.PutStreamAsync(stream, metadata, uploadProgress);
                    UploadProgress.Value = 100;

                    var downloadProgress = new Progress<IDownloadState>();
                    downloadProgress.ProgressChanged += (sender, e) =>
                    {
                        DownloadProgress.Value = e.TotalByteCount > 0 ? 100.0 * e.BytesTransferred / e.TotalByteCount : 0;
                    };

                    var data = await reference.GetStreamAsync(downloadProgress);
                    DownloadProgress.Value = 100;

                    Image.Value = ImageSource.FromStream(() =>
                    {
                        return data;
                    });
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }
            });
        }
    }
}
