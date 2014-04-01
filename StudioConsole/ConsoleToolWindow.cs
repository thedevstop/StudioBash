using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms.Integration;
using TheDevStop.StudioConsole.Settings;

namespace TheDevStop.StudioConsole
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
    /// implementation of the IVsUIElementPane interface.
    /// </summary>
    [Guid("6e40be5b-ee19-4f2d-9a93-32f2235c9aed")]
    public class ConsoleToolWindow : ToolWindowPane
    {
        private ApplicationControl ConsoleControl { get; set; }

        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public ConsoleToolWindow() :
            base(null)
        {
            // Set the window title reading it from the resources.
            this.Caption = Resources.ToolWindowTitle;
            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.e
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;

            if (string.IsNullOrEmpty(SCSettings.Instance.ConsolePath))
                return;

            ConsoleControl = new ApplicationControl();

            // Replace this with user configuration
            ConsoleControl.ExeName = SCSettings.Instance.ConsolePath; // @"C:\Program Files (x86)\Console2\console.exe";

            // The Bash shell is a fixed size console set through properties. Better to embed conEmu (cmder) or console2.
            //consoleControl.ExeName = @"C:\Program Files (x86)\Git\bin\sh.exe";
            //consoleControl.Args = "--login -i";

            // Create the interop host control.
            WindowsFormsHost host = new WindowsFormsHost();

            // Assign the Console control as the host control's child.
            host.Child = ConsoleControl;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
            // the object returned by the Content property.
            base.Content = host;
        }
    }
}