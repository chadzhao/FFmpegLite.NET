# FFmpegLite.NET
A lightweight & extentable wrapper of FFmpeg.

I used [FFmpeg.NET](https://github.com/cmxl/FFmpeg.NET) as my media lib, but it is not that good as I thought. So I made [FFmpegLite.NET](https://github.com/chadzhao/FFmpegLite.NET), major parts token from FFmpeg.NET, but has more features

- clear arguments with task (groupped by convert/get thumbnail/get metadata)
- easy to extend by adding extra command
- merge PR in-time :smile:

**Important:** It is still in pre-release stage, recommend to use from v1.0.

## Features
- Use fluent syntax
- Easy to extend

## Packages

| Package | NuGet |
| --- | --- |
| FFmpegLite.NET | [![NuGet](https://buildstats.info/nuget/FFmpegLite.NET)](https://www.nuget.org/packages/FFmpegLite.NET) |

## Get started!

Install [FFmpegLite.NET](https://github.com/chadzhao/FFmpegLite.NET) from nuget.org Package Source using the Package Manager Console with the following command

    PM> Install-Package FFmpegLite.NET

## Samples

``` C#
// set default enviroment for Windows OS
FFmpegEnviroment.SetPath(@"C:\ffmpeg\bin\ffmpeg.exe"); // linux/ubuntu with ffmpeg installed just skip this step

// convert video
var convertResult = await new FFmpegConvertTask()
    .FromFile(@"C:\Path\To_Video.flv") // set input file
    .Resize(null, 720) // set output frame size
    .AppendExtraCommand(" -movflags +faststart ") // can add extra arguments 
    .ConvertAsync("NewVideo.mp4"); // start task

// get thumbnail
var thumbailFile = await new FFmpegThumbnailTask()
    .FromFile("video.mp4")
    .GetThumbnailAsync("thumbnail.jpg");

// get meta data
var metadata = await new FFmpegMetadataTask()
    .FromFile("video.mp4")
    .GetMetadataAsync();

```

## Extend

It is very easy to extend this lib.

#### FFmpegTask.AppendExtraCommand()

``` C#
var convertResult = await new FFmpegConvertTask(ffmpeg)
    .FromFile(@"C:\Path\To_Video.flv")
    .AppendExtraCommand(" -movflags +faststart ") // can add extra arguments 
    .ConvertAsync("NewVideo.mp4");
```

#### Add Extension Method
``` C#
public static class MyFFmpegLiteExtension
{
    public static FFmpegConvertTask UseFaststartFlags(this FFmpegConvertTask convertTask)
    {
        return convertTask.AppendExtraCommand(" -movflags +faststart ");
    }
}

var rst = await new FFmpegConvertTask()
    .FromFile(@"C:\Path\To_Video.flv")
    .UseFaststartFlags()
    .ConvertAsync("NewVideo.mp4");
```

## Contribution

You are encouraged to contribute this project by 
- Report issues
- Submit requests
- PR
