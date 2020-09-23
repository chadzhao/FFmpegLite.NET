using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FFmpegLite.NET
{
    /// <summary>
    /// Extension methods to extend FFmpegConvertTask
    /// </summary>
    public static class FFmpegConvertTaskExtensions
    {
        /// <summary>
        /// Use H.264 Baseline Profile
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static TTask UseBaselineProfile<TTask>(this TTask task) where TTask : FFmpegConvertTask
        {
            task.AppendCommand(" -profile:v baseline ");
            return task;
        }

        /// <summary>
        /// Resize frame size
        /// </summary>
        /// <param name="task"></param>
        /// <param name="width">Leave it null if want auto scale with height</param>
        /// <param name="height">Leave it null if want auto scale with width</param>
        /// <returns></returns>
        public static TTask Resize<TTask>(this TTask task, int? width, int? height) where TTask : FFmpegConvertTask
        {
            if (width.HasValue || height.HasValue)
            {
                task.AppendCommand(" -vf \"scale={0}:{1}\" ", width ?? -2, height ?? -2);
            }

            return task;
        }

        /// <summary>
        /// set video fps
        /// </summary>
        /// <param name="task"></param>
        /// <param name="fps"></param>
        /// <returns></returns>
        public static TTask Fps<TTask>(this TTask task, int fps) where TTask: FFmpegConvertTask
        {
            task.AppendCommand($" -r {fps} ");
            return task;
        }

        /// <summary>
        /// set audio bit rate
        /// </summary>
        /// <param name="task"></param>
        /// <param name="audioBitRate"></param>
        /// <returns></returns>
        public static TTask AudioBitRate<TTask>(this TTask task, int audioBitRate) where TTask: FFmpegConvertTask
        {
            task.AppendCommand($" -ab {audioBitRate}k ");
            return task;
        }

        /// <summary>
        /// Use faststart flags for mp4 video for better online play
        /// </summary>
        /// <typeparam name="TTask"></typeparam>
        /// <param name="task"></param>
        /// <returns></returns>
        public static TTask UseFastStartMode<TTask>(this TTask task) where TTask: FFmpegConvertTask
        {
            task.AppendCommand(" -movflags +faststart ");
            return task;
        }

        /// <summary>
        /// start to convert file
        /// </summary>
        /// <param name="task"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static async Task<FileInfo> ConvertAsync(this FFmpegConvertTask task, string outputFile, CancellationToken cancellationToken = default)
        {
            return await ConvertAsync(task, outputFile, FFmpegEnviroment.Default, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// start to convert file
        /// </summary>
        /// <param name="task"></param>
        /// <param name="outputFile"></param>
        /// <returns></returns>
        public static async Task<FileInfo> ConvertAsync(this FFmpegConvertTask task, string outputFile, FFmpegEnviroment enviroment, CancellationToken cancellationToken = default)
        {
            task.AppendCommand($" \"{outputFile}\" ");

            var process = new FFmpegProcess();
            await process.ExecuteAsync(task, enviroment, cancellationToken: cancellationToken);

            return task.OutputFile;
        }
    }
}
