using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FFmpegLite.NET
{
    /// <summary>
    /// Extension methods to extend FFmpegMetadataTask
    /// </summary>
    public static class FFmpegMetadataTaskExtensions
    {
        /// <summary>
        /// Use H.264 Baseline Profile
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static TTask UseBaselineProfile<TTask>(this TTask task) where TTask : FFmpegMetadataTask
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
        public static TTask Resize<TTask>(this TTask task, int? width, int? height) where TTask : FFmpegMetadataTask
        {
            if (width.HasValue || height.HasValue)
            {
                task.AppendCommand(" -vf \"scale={0}:{1}\" ", width ?? -2, height ?? -2);
            }

            return task;
        }


        /// <summary>
        /// start to get meta data
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<MetaData> GetMetadataAsync(this FFmpegMetadataTask task, CancellationToken cancellationToken = default)
        {
            return await GetMetadataAsync(task, FFmpegEnviroment.Default, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// start to get meta data
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static async Task<MetaData> GetMetadataAsync(this FFmpegMetadataTask task, FFmpegEnviroment enviroment, CancellationToken cancellationToken = default)
        {
            task.AppendCommand(" -f ffmetadata - ");

            var process = new FFmpegProcess();
            await process.ExecuteAsync(task, enviroment, cancellationToken: cancellationToken);

            return task.MetaData;
        }
    }
}
