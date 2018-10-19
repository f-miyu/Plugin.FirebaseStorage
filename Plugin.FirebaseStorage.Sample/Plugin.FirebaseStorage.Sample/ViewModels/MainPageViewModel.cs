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

namespace Plugin.FirebaseStorage.Sample.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public AsyncReactiveCommand PickPhotmCommand { get; } = new AsyncReactiveCommand();
        public ReactivePropertySlim<ImageSource> Image { get; } = new ReactivePropertySlim<ImageSource>();

        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
            : base(navigationService)
        {
            Title = "Main Page";

            PickPhotmCommand.Subscribe(async () =>
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

                    var reference = CrossFirebaseStorage.Current.Storage.RootReference.GetChild(name);

                    var path = reference.Path;

                    var progress = new Progress<IUploadState>();
                    progress.ProgressChanged += (sender, e) =>
                    {
                        System.Diagnostics.Debug.WriteLine($"{e.BytesTransferred}, {e.TotalByteCount}, {file.GetStream().Length}");
                    };

                    var metadata = new MetadataChange
                    {
                        ContentType = "image/jpeg"
                    };

                    var cts = new CancellationTokenSource();
                    var pts = new PauseTokenSource();

                    var task = reference.PutStreamAsync(file.GetStream(), metadata, progress, cts.Token, pts.Token);
                    await Task.Delay(100);
                    pts.Resume();
                    pts.Pause();
                    pts.Pause();

                    await Task.Delay(3000);

                    pts.Resume();

                    await task;

                    var progress2 = new Progress<IDownloadState>();
                    progress2.ProgressChanged += (sender, e) =>
                    {
                        System.Diagnostics.Debug.WriteLine($"{e.BytesTransferred}, {e.TotalByteCount}");
                    };


                    var cts2 = new CancellationTokenSource();
                    var task2 = reference.GetStreamAsync(progress2, cts2.Token);

                    //await Task.Delay(900);

                    //cts2.Cancel();

                    var data = await task2;

                    Image.Value = ImageSource.FromStream(() =>
                    {
                        return data;
                    });
                }
                catch (Exception e)
                {
                    await pageDialogService.DisplayAlertAsync("error", e.ToString(), "OK");
                }
                finally
                {
                    file?.Dispose();
                }

            });
        }
    }
}
