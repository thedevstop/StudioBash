using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TheDevStop.StudioBash
{
    public class Bash : INotifyPropertyChanged, IDisposable
    {
        private bool _isDisposed = false;
        private readonly StreamWriter _writer;

        public event DataReceivedEventHandler OutputDataReceived;

        public string ContentCache { get; set; }

        public Bash()
        {
            var path = @"C:\Program Files (x86)\Git\bin\sh.exe";
            var args = "--login -i";

            var processStartInfo = new ProcessStartInfo(path, args);
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.UseShellExecute = false;

            var process = Process.Start(processStartInfo);
            process.OutputDataReceived += process_OutputDataReceived;
            process.BeginOutputReadLine();

            _writer = process.StandardInput;
        }

        public async Task SendLine(string input)
        {
            this.ContentCache += System.Environment.NewLine + "> " + input;
            await _writer.WriteLineAsync(input);
        }

        void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (this.OutputDataReceived != null)
                this.OutputDataReceived(this, e);

            // TODO: Enforce max length
            this.ContentCache += System.Environment.NewLine + e.Data;
            this.Notify("ContentCache");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (_isDisposed)
                return;

            if (isDisposing)
            {
                if (_writer != null)
                    _writer.Dispose();
            }

            _isDisposed = true;
        }

        ~Bash()
        {
            this.Dispose(false);
        }
    }
}
