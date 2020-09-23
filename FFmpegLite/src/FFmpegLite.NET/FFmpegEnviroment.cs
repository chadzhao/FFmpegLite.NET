using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FFmpegLite.NET
{
    /// <summary>
    /// Enviroment for FFmpeg
    /// </summary>
    public sealed class FFmpegEnviroment
    {
        /// <summary>
        /// Default Enviroment
        /// </summary>
        public static FFmpegEnviroment Default { get; } = new FFmpegEnviroment();

        /// <summary>
        /// FFmpeg exe path or just ffmpeg command
        /// </summary>
        public string FFmpegPath { get; private set; }

        public FFmpegEnviroment(string ffmpegPath = "ffmpeg")
        {
            this.FFmpegPath = ffmpegPath;
        }

        public static void SetPath(string path)
        {
            Default.FFmpegPath = path;
        }
    }
}