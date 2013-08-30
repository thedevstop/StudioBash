using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.Shell;

namespace TheDevStop.StudioBash
{
    /// <summary>
    /// Interaction logic for MyControl.xaml
    /// </summary>
    public partial class MyControl : UserControl
    {
        private readonly Bash _bash;

        public MyControl()
        {
            InitializeComponent();

            _bash = new Bash();
            _bash.OutputDataReceived += _bash_OutputDataReceived;
            this.DataContext = _bash;
        }

        void _bash_OutputDataReceived(object sender, System.Diagnostics.DataReceivedEventArgs e)
        {
            ContentScroller.ScrollToEnd();
        }

        private async void Command_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            await _bash.SendLine(this.Command.Text);
            this.Command.Text = string.Empty;
        }       
    }
}