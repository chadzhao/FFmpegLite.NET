# FFmpegLite.NET
A lightweight & extentable wrapper of FFmpeg


# Features
- Use fluent syntax
- Easy to extend

## Packages

| Package | NuGet |
| --- | --- |
| FFmpegLite.NET | [![NuGet](https://buildstats.info/nuget/FFmpegLite.NET)](https://www.nuget.org/packages/FFmpegLite.NET) |

# Samples

``` C#
// set Engine
var ffmpeg = new Engine("C:\\ffmpeg\\ffmpeg.exe");

// convert video
var convertResult = await new ConvertTask(ffmpeg)
    .FromFile(@"C:\Path\To_Video.flv") // set input file
    .Resize(1920, 720) // set output frame size
    .SeekVideo(TimeSpan.FromSeconds(10)) // start from 10 seconds
    .MaxVideoDuration(TimeSpan.FromSeconds(30)) // set duration
    .AudioSampleRate(AudioSampleRate.Hz44100) // set audio sample rate
    .CropVideo(100, 200) // Crop video from point (100, 200)
    .Fps(24) // use FPS 24
    .ExtraArgument("-movflags +faststart") // can add extra arguments 
    .ConvertAsync(@"C:\Path\To_Save_New_Video.mp4"); // start task
```

# Extend

It is very easy to extend this lib.

#### ConvertTask.ExtraArgument()

``` C#
var convertResult = await new ConvertTask(ffmpeg)
    .FromFile(@"C:\Path\To_Video.flv")
    .ExtraArgument("-movflags +faststart") // can add extra arguments 
    .ConvertAsync(@"C:\Path\To_Save_New_Video.mp4");
```

#### Add Extension Method
``` C#
public static class MyFFmpegLiteExtension
{
    public static ConvertTask NewMethod(this ConvertTask convertTask, object arg1)
    {
        
    }
}

var rst = await convertTask
    .FromFile(@"C:\Path\To_Video.flv")
    .NewMethod()
    .ConvertAsync(@"C:\Path\To_Save_New_Video.mp4");
```

# Contribution

You are encouraged to contribute this project by 
- Report issues
- Submit requests
- PR
