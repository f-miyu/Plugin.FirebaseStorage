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
        public AsyncReactiveCommand PickPhotmCommand { get; } = new AsyncReactiveCommand();
        public ReactivePropertySlim<ImageSource> Image { get; } = new ReactivePropertySlim<ImageSource>();

        public MainPageViewModel(INavigationService navigationService)
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

                Image.Value = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            });
        }
    }
}
