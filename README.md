# Plugin.FirebaseStorage

A cross platform plugin for Firebase Storage. 
A wrapper for [Xamarin.Firebase.iOS.Storage](https://www.nuget.org/packages/Xamarin.Firebase.iOS.Storage/) 
and [Xamarin.Firebase.Storage](https://www.nuget.org/packages/Xamarin.Firebase.Storage/).

## Setup
Install Nuget package to each projects.

[Plugin.FirebaseStorage](https://www.nuget.org/packages/Plugin.FirebaseStorage/) [![NuGet](https://img.shields.io/nuget/vpre/Plugin.FirebaseStorage.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.FirebaseStorage/)

### iOS
* Add GoogleService-Info.plist to iOS project. Select BundleResource as build action.
* Initialize as follows in AppDelegate. 
```C#
Firebase.Core.App.Configure();
```

### Android
* Add google-services.json to Android project. Select GoogleServicesJson as build action. (If you can't select GoogleServicesJson, reload this android project.)
* Target framework version needs to be Android 10.0.
* Add the following into AndroidManifest.xml
```xml
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
```

## Usage
### Update from bytes
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

await reference.PutBytesAsync(bytes);
```

### Update from a stream
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

await reference.PutStreamAsync(stream);
```

### Update from a local file
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

await reference.PutFileAsync(filePath);
```

### Upload with metadata
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

var metadata = new MetadataChange
{
    ContentType = "image/jpeg"
};

await reference.PutStreamAsync(stream, metadata);
```

### Monitor upload progress
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

var uploadProgress = new Progress<IUploadState>();
uploadProgress.ProgressChanged += (sender, e) =>
{
    var progress = e.TotalByteCount > 0 ? 100.0 * e.BytesTransferred / e.TotalByteCount : 0;
};

await reference.PutStreamAsync(stream, progress: uploadProgress);
```

### Cancel upload
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

var cts = new CancellationTokenSource();
var task = Task.Run(async () =>
{
     await Task.Delay(1000);
     
     cts.Cancel();
});

await reference.PutStreamAsync(stream, cancellationToken: cts.Token);
```

### Pause upload
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

var pts = new PauseTokenSource();
var task = Task.Run(async () =>
{
     await Task.Delay(1000);
     
     pts.Pause();
     
     await Task.Delay(1000);
     
     pts.Resume();
});

await reference.PutStreamAsync(stream, pauseToken: pts.Token);
```

### Download to bytes
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

var maxDownloadSizeBytes = 1024 * 1024;

var bytes = await reference.GetBytesAsync(maxDownloadSizeBytes);
```

### Download to a stream
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

var stream = await reference.GetStreamAsync();
```

### Download to a local file
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

await reference.GetFileAsync(filePath);
```

### Monitor download progress
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

var downloadProgress = new Progress<IDownloadState>();
downloadProgress.ProgressChanged += (sender, e) =>
{
    var progress = e.TotalByteCount > 0 ? 100.0 * e.BytesTransferred / e.TotalByteCount : 0;
};

var stream = await reference.GetStreamAsync(downloadProgress);
```

### Cancel download
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

var cts = new CancellationTokenSource();
var task = Task.Run(async () =>
{
     await Task.Delay(1000);
     
     cts.Cancel();
});

var stream = await reference.GetStreamAsync(cancellationToken: cts.Token);
```

### Get a download URL
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

var url = await reference.GetDownloadUrlAsync();
```

### Get metadata
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

var metadata = await reference.GetMetadataAsync();
```

### Update metadata
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

var metadata = new MetadataChange
{
    ContentType = "image/jpeg",
    CustomMetadata = new Dictionary<string, string>
    {
        ["myCustomProperty"] = "myValue"
    }
};

await reference.UpdateMetadataAsync(metadata);
```

### Delete a file
```C#
var reference = CrossFirebaseStorage.Current.Instance.RootReference.Child("image.jpg");

await reference.DeleteAsync();
```

### List files
```C#
// All
var result = await CrossFirebaseStorage.Current.Instance.RootReference.ListAllAsync();
var items = result.Items.ToList();

// Pagination
var result1 = await CrossFirebaseStorage.Current.Instance.RootReference.ListAsync(10);
var token = result1.PageToken;
var result2 = await CrossFirebaseStorage.Current.Instance.RootReference.ListAsync(10, token);
```

### Use multiple projects
```C#
var reference = CrossFirebaseStorage.Current.GetInstance("SecondAppName").RootReference.Child("image.jpg");

await reference.PutBytesAsync(bytes);
```
