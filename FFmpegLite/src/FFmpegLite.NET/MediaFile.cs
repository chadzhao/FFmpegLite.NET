using System;
using System.IO;

namespace FFmpegLite.NET
{
    public class MediaFile
    {
        public MediaFile(string file) : this(new FileInfo(file))
        {
        }

        public MediaFile(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }

        public FileInfo FileInfo { get; }
        internal MetaData MetaData { get; set; }
    }
}
