using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace FFmpegLite.NET
{
    internal sealed class FFmpegProcess
    {
        public async Task ExecuteAsync(FFmpegTask ffmpegTask, FFmpegEnviroment enviroment, CancellationToken cancellationToken = default)
        {
            var startInfo = new ProcessStartInfo
            {
                // -y overwrite output files
                Arguments = "-y " + ffmpegTask.GetCommandString(),
                FileName = enviroment.FFmpegPath,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            await ExecuteAsync(startInfo, ffmpegTask, cancellationToken);
        }

        private async Task ExecuteAsync(ProcessStartInfo startInfo, FFmpegTask ffmpegTask, CancellationToken cancellationToken = default)
        {
            var messages = new List<string>();
            Exception caughtException = null;

            using (var ffmpegProcess = new Process() { StartInfo = startInfo })
            {
                //ffmpegProcess.ErrorDataReceived += (sender, e) => OnData(new ConversionDataEventArgs(e.Data, parameters.InputFile, parameters.OutputFile));
                ffmpegProcess.ErrorDataReceived += (sender, e) => FFmpegProcessOnErrorDataReceived(e, ffmpegTask, ref caughtException, messages);

                Task<int> task = null;
                try
                {
                    task = ffmpegProcess.WaitForExitAsync(null, cancellationToken);
                    await task;
                }
                catch (Exception)
                {
                    // An exception occurs if the user cancels the operation by calling Cancel on the CancellationToken.
                    // Exc.Message will be "A task was canceled." (in English).
                    // task.IsCanceled will be true.
                    if (task.IsCanceled)
                    {
                        throw new TaskCanceledException(task);
                    }
                    // I don't think this can occur, but if some other exception, rethrow it.
                    throw;
                }

                //if (caughtException != null || ffmpegProcess.ExitCode != 0)
                //{
                //    OnException(messages, parameters, ffmpegProcess.ExitCode, caughtException);
                //}
                //else
                //{
                //    OnConversionCompleted(new ConversionCompleteEventArgs(parameters.InputFile, parameters.OutputFile));
                //}
            }
        }

        //private void OnException(List<string> messages, FFmpegParameters parameters, int exitCode, Exception caughtException)
        //{
        //    var exceptionMessage = GetExceptionMessage(messages);
        //    var exception = new FFmpegException(exceptionMessage, caughtException, exitCode);
        //    OnConversionError(new ConversionErrorEventArgs(exception, parameters.InputFile, parameters.OutputFile));
        //}

        //private string GetExceptionMessage(List<string> messages)
        //    => messages.Count > 1
        //        ? messages[1] + messages[0]
        //        : string.Join(string.Empty, messages);

        private void FFmpegProcessOnErrorDataReceived(DataReceivedEventArgs e, FFmpegTask ffmpegTask, ref Exception exception, List<string> messages)
        {
            var totalMediaDuration = new TimeSpan();
            if (e.Data == null)
                return;

            try
            {
                messages.Insert(0, e.Data);

                if (ffmpegTask.MetaData == null) ffmpegTask.MetaData = new MetaData();

                RegexEngine.TestVideo(e.Data, ffmpegTask);
                RegexEngine.TestAudio(e.Data, ffmpegTask);

                var matchDuration = RegexEngine._index[RegexEngine.Find.Duration].Match(e.Data);
                if (matchDuration.Success)
                {
                    if (ffmpegTask.MetaData == null) ffmpegTask.MetaData = new MetaData();

                    RegexEngine.TimeSpanLargeTryParse(matchDuration.Groups[1].Value, out totalMediaDuration);
                    ffmpegTask.MetaData.Duration = totalMediaDuration;
                }

                if (RegexEngine.IsProgressData(e.Data, out var progressData))
                {
                    //if (parameters.InputFile != null)
                    //{
                    progressData.TotalDuration = ffmpegTask.MetaData?.Duration ?? totalMediaDuration;
                    //}

                    //OnProgressChanged(new ConversionProgressEventArgs(progressData, parameters.InputFile, parameters.OutputFile));
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        //public event Action<ConversionProgressEventArgs> Progress;
        //public event Action<ConversionCompleteEventArgs> Completed;
        //public event Action<ConversionErrorEventArgs> Error;
        //public event Action<ConversionDataEventArgs> Data;

        //private void OnProgressChanged(ConversionProgressEventArgs eventArgs) => Progress?.Invoke(eventArgs);

        //private void OnConversionCompleted(ConversionCompleteEventArgs eventArgs) => Completed?.Invoke(eventArgs);

        //private void OnConversionError(ConversionErrorEventArgs eventArgs) => Error?.Invoke(eventArgs);

        //private void OnData(ConversionDataEventArgs eventArgs) => Data?.Invoke(eventArgs);
    }
}
