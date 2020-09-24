using FFmpegLite.NET.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
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
        public static TTask Fps<TTask>(this TTask task, int fps) where TTask : FFmpegConvertTask
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
        public static TTask AudioBitRate<TTask>(this TTask task, int audioBitRate) where TTask : FFmpegConvertTask
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
        public static TTask UseFastStartMode<TTask>(this TTask task) where TTask : FFmpegConvertTask
        {
            task.AppendCommand(" -movflags +faststart ");
            return task;
        }

        /// <summary>
        /// Set Video Aspect Ratio
        /// </summary>
        /// <typeparam name="TTask"></typeparam>
        /// <param name="task"></param>
        /// <param name="videoAspectRatio"></param>
        /// <returns></returns>
        public static TTask VideoAspectRatio<TTask>(this TTask task, VideoAspectRatio videoAspectRatio) where TTask : FFmpegConvertTask
        {
            var ratio = videoAspectRatio.ToString();
            ratio = ratio.Substring(1);
            ratio = ratio.Replace("_", ":");

            task.AppendCommand(" -aspect {0} ", ratio);

            return task;
        }

        /// <summary>
        /// Crop video
        /// </summary>
        /// <typeparam name="TTask"></typeparam>
        /// <param name="task"></param>
        /// <param name="videoCrop"></param>
        /// <returns></returns>
        public static TTask Crop<TTask>(this TTask task, Rectangle videoCrop) where TTask : FFmpegConvertTask
        {
            task.AppendCommand(" -filter:v \"crop={0}:{1}:{2}:{3}\" ", videoCrop.Width, videoCrop.Height, videoCrop.X, videoCrop.Y);

            return task;
        }

        /// <summary>
        /// Set video bit rate
        /// </summary>
        /// <typeparam name="TTask"></typeparam>
        /// <param name="task"></param>
        /// <param name="bitRate"></param>
        /// <returns></returns>
        public static TTask VideoBitRate<TTask>(this TTask task, int bitRate) where TTask : FFmpegConvertTask
        {
            task.AppendCommand(" -b {0}k ", bitRate);

            return task;
        }

        /// <summary>
        /// The frame to begin seeking from
        /// </summary>
        /// <typeparam name="TTask"></typeparam>
        /// <param name="task"></param>
        /// <param name="seek"></param>
        /// <returns></returns>
        public static TTask Seek<TTask>(this TTask task, TimeSpan seek) where TTask : FFmpegConvertTask
        {
            task.AppendCommand(CultureInfo.InvariantCulture, " -ss {0} ", seek.TotalSeconds);

            return task;
        }

        /// <summary>
        /// Set max video duration
        /// </summary>
        /// <typeparam name="TTask"></typeparam>
        /// <param name="task"></param>
        /// <param name="maxVideoDuration"></param>
        /// <returns></returns>
        public static TTask VideoMaxDuration<TTask>(this TTask task, TimeSpan maxVideoDuration) where TTask : FFmpegConvertTask
        {
            task.AppendCommand(" -t {0} ", maxVideoDuration);

            return task;
        }

        /// <summary>
        /// Set audio sample rate
        /// </summary>
        /// <typeparam name="TTask"></typeparam>
        /// <param name="task"></param>
        /// <param name="audioSampleRate"></param>
        /// <returns></returns>
        public static TTask AudioSampleRate<TTask>(this TTask task, AudioSampleRate audioSampleRate) where TTask : FFmpegConvertTask
        {
            task.AppendCommand(" -ar {0} ", audioSampleRate.ToString().Replace("Hz", ""));

            return task;
        }

        /// <summary>
        /// Set video target, for physical media conversion (DVD etc)
        /// </summary>
        /// <typeparam name="TTask"></typeparam>
        /// <param name="task"></param>
        /// <param name="target"></param>
        /// <param name="targetStandard"></param>
        /// <returns></returns>
        public static TTask Target<TTask>(this TTask task, Target target, TargetStandard? targetStandard = null) where TTask : FFmpegConvertTask
        {
            task.AppendCommand(" -target ");

            if (targetStandard.HasValue)
            {
                task.AppendCommand(" {0}-{1}", targetStandard.ToString().ToLowerInvariant(), target.ToString().ToLowerInvariant());
            }
            else
            {
                task.AppendCommand("{0} ", target.ToString());
            }

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

            task.OutputFile = new FileInfo(outputFile);

            return task.OutputFile;
        }
    }
}
