using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FFmpegLite.NET
{
    /// <summary>
    /// Extension methods to extend FFmpegTask
    /// </summary>
    public static class FFmpegTaskExtensions
    {
        /// <summary>
        /// set input file
        /// </summary>
        /// <param name="task"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static TTask FromFile<TTask>(this TTask task, string filePath) where TTask : FFmpegTask
        {
            task.AppendCommand($" -i \"{filePath}\" ");
            return task;
        }

        /// <summary>
        /// Append extra command (Recommend to PR if you think it should be must-have feature :-) )
        /// </summary>
        /// <param name="task"></param>
        /// <param name="extraCommand"></param>
        /// <returns></returns>
        public static TTask AppendExtraCommand<TTask>(this TTask task, string extraCommand) where TTask : FFmpegTask
        {
            task.AppendCommand(extraCommand);
            return task;
        }
    }
}
