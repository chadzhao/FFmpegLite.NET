using System;
using System.Diagnostics;
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
            Console.WriteLine("FFmpegLite Test");

            FFmpegEnviroment.SetPath(@"D:\download\ffmpeg-20200831-4a11a6f-win64-static\bin\ffmpeg.exe");

            var inputVideo = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MediaFiles", "SampleVideo_1280x720_1mb.mp4"));
            //var ouputThumbnail = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "thumbnail.jpg"));

            var sw = new Stopwatch();
            sw.Start();

            var thumbnailFile = await new FFmpegThumbnailTask()
                .FromFile(inputVideo.FullName)
                .GetThumbnailAsync("thumbnail.jpg");

            sw.Stop();
            Console.WriteLine($"took {sw.Elapsed.TotalSeconds} seconds for thumbnail");

            sw.Restart();

            var metadataResult = await new FFmpegMetadataTask()
                .FromFile(inputVideo.FullName)
                .GetMetadataAsync();

            sw.Stop();
            Console.WriteLine($"took {sw.Elapsed.TotalSeconds} seconds for meta data : {metadataResult.ToString()}");

            sw.Restart();

            var convertFile = await new FFmpegConvertTask()
                .FromFile(inputVideo.FullName)
                .Seek(TimeSpan.FromSeconds(1))
                .Resize(null, 480)
                .UseFastStartMode()
                .UseBaselineProfile()
                .ConvertAsync("480p.mp4");

            sw.Stop();

            Console.WriteLine($"took {sw.Elapsed.TotalSeconds} seconds for convert : {convertFile.Name}, size : {convertFile.Length}");
        }
    }
}
