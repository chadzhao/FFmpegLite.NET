using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FFmpegLite.NET
{
    /// <summary>
    /// Define a command to run common FFmpeg tasks (convert/get meta data/get thumbnail)
    /// </summary>
    public class FFmpegTask
    {
        protected StringBuilder commandBuilder;

        internal FileInfo OutputFile { get; set; }
        internal MetaData MetaData { get; set; }

        public FFmpegTask()
        {
            this.commandBuilder = new StringBuilder();
        }

        /// <summary>
        /// Append command,  such as " -movflags +faststart "
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal FFmpegTask AppendCommand(string command, params object[] args)
        {
            this.commandBuilder.AppendFormat(command, args);
            return this;
        }

        /// <summary>
        /// Append command,  such as " -movflags +faststart "
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        internal FFmpegTask AppendCommand(IFormatProvider provider, string command, params object[] args)
        {
            this.commandBuilder.AppendFormat(provider, command, args);
            return this;
        }

        /// <summary>
        /// Get final command string
        /// </summary>
        /// <returns></returns>
        internal string GetCommandString()
        {
            return this.commandBuilder.ToString();
        }
    }
}
