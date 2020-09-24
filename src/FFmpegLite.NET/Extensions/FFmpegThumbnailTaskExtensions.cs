using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FFmpegLite.NET
{
    /// <summary>
    /// Extension methods to extend FFmpegMetadataTask
    /// </summary>
    public static class FFmpegThumbnailTaskExtensions
    {
        /// <summary>
        /// Resize frame size
        /// </summary>
        /// <param name="task"></param>
        /// <param name="width">Leave it null if want auto scale with height</param>
        /// <param name="height">Leave it null if want auto scale with width</param>
        /// <returns></returns>
        public static TTask Resize<TTask>(this TTask task, int? width, int? height) where TTask : FFmpegThumbnailTask
        {
            if (width.HasValue || height.HasValue)
            {
                task.AppendCommand(" -vf \"scale={0}:{1}\" ", width ?? -2, height ?? -2);
            }

            return task;
        }

        /// <summary>
        /// start to get thumbnail
        /// </summary>
        /// <param name="task"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static async Task<FileInfo> GetThumbnailAsync(this FFmpegThumbnailTask task, string outputFile, CancellationToken cancellationToken = default)
        {
            return await GetThumbnailAsync(task, outputFile, FFmpegEnviroment.Default, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// start to get thumbnail
        /// </summary>
        /// <param name="task"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static async Task<FileInfo> GetThumbnailAsync(this FFmpegThumbnailTask task, string outputFile, FFmpegEnviroment enviroment, CancellationToken cancellationToken = default)
        {
            task.OutputFile = new FileInfo(outputFile);

            task.AppendCommand(CultureInfo.InvariantCulture, " -ss {0} ", TimeSpan.FromSeconds(0));
            task.AppendCommand(" -vframes {0} ", 1);
            task.AppendCommand($" \"{outputFile}\" ");

            var process = new FFmpegProcess();
            await process.ExecuteAsync(task, enviroment, cancellationToken: cancellationToken);

            return task.OutputFile;
        }
    }
}
