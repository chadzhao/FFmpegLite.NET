using System;
using System.IO;
using System.Threading.Tasks;
using FFmpegLite;
using FFmpegLite.NET;

namespace FFmpegLiteSamples
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            FFmpegEnviroment.SetPath(@"D:\download\ffmpeg-20200831-4a11a6f-win64-static\bin\ffmpeg.exe");

            var inputVideo = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MediaFiles", "SampleVideo_1280x720_1mb.mp4"));
            //var ouputThumbnail = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "thumbnail.jpg"));

            var thumbnailFile = await new FFmpegThumbnailTask()
                .FromFile(inputVideo.FullName)
                .GetThumbnailAsync("thumbnail.jpg");

            var metadataResult = await new FFmpegMetadataTask()
                .FromFile(inputVideo.FullName)
                .GetMetadataAsync();

            Console.WriteLine($"meta data : {metadataResult.ToString()}");

            var convertFile = await new FFmpegConvertTask()
                .FromFile(inputVideo.FullName)
                .UseBaselineProfile()
                .Resize(null, 480)
                .UseFastStartMode()
                .ConvertAsync("480p.mp4");
        }
    }
}
