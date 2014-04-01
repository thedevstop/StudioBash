using System;
using System.Diagnostics;

namespace TheDevStop.StudioConsole
{
    /// <summary>
    /// Application Display Control
    /// </summary>
    public class ApplicationControl : System.Windows.Forms.Panel
	{
		public event EventHandler ApplicationExited;

		/// <summary>
		/// Handle to the application Window
		/// </summary>
		IntPtr appHwnd = IntPtr.Zero;

		/// <summary>
		/// The name of the exe to launch
		/// </summary>
        public string ExeName { get; set; }

        /// <summary>
        /// The args for the exe to launch
        /// </summary>
        public string Args { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public ApplicationControl()
		{
		}

        /// <summary>
        /// When this control receives focus then focus the hosted application
        /// </summary>
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            this.OnResize(e);

            NativeMethods.SetForegroundWindow(appHwnd);
            NativeMethods.SetActiveWindow(appHwnd);
            NativeMethods.SetFocus(appHwnd);
        }

		/// <summary>
		/// Force redraw of control when size changes
		/// </summary>
		protected override void OnSizeChanged(EventArgs e)
		{
			this.Invalidate();
			base.OnSizeChanged(e);
		}

		/// <summary>
		/// Creeate control when visibility changes
		/// </summary>
		protected override void OnVisibleChanged(EventArgs e)
		{
			// If control needs to be initialized/created
            if (appHwnd == IntPtr.Zero)
			{
				// Start the remote application
                System.Diagnostics.Process p = null;
				try
				{
                    var processStartInfo = new ProcessStartInfo(ExeName, Args);

					// Start the process
                    p = System.Diagnostics.Process.Start(processStartInfo);
					p.Exited += OnExited;

					// Wait for process to be created and enter idle condition
					// p.WaitForInputIdle();
                    while (p.MainWindowHandle == IntPtr.Zero)
                    {
                        System.Threading.Thread.Sleep(10);
                        p.Refresh();
                    }

					// Get the main handle
					appHwnd = p.MainWindowHandle;
				}
				catch (Exception ex)
				{
                    if (p != null)
                        p.Dispose();

					return;
				}

				// Put it into this form
                NativeMethods.SetParent(appHwnd, this.Handle);

                // Make the window a Layered Window to fix Transparency issue on Win7
                var style = NativeMethods.GetWindowLong(appHwnd, NativeMethods.GWL_EXSTYLE);
                NativeMethods.SetWindowLong(appHwnd, NativeMethods.GWL_EXSTYLE, style ^ NativeMethods.WS_EX_LAYERED);

				// Remove border and whatnot
                NativeMethods.SetWindowLong(appHwnd, NativeMethods.GWL_STYLE, NativeMethods.WS_VISIBLE);

				// Move the window to overlay it on this window
                NativeMethods.MoveWindow(appHwnd, 0, 0, this.Width, this.Height, true);
			}

			base.OnVisibleChanged(e);
		}

        /// <summary>
        /// When the thread exits notify others
        /// </summary>
		void OnExited(object owner, EventArgs args)
		{
			if (ApplicationExited != null)
				ApplicationExited(this, args);
		}

		/// <summary>
		/// Close the hosted application when we are disposed
		/// </summary>
		protected override void OnHandleDestroyed(EventArgs e)
		{
			// Stop the application
			if (appHwnd != IntPtr.Zero)
			{
				// Post a close message
                NativeMethods.PostMessage(appHwnd, NativeMethods.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

				// Delay for it to get the message
				System.Threading.Thread.Sleep(1000);

				// Clear internal handle
				appHwnd = IntPtr.Zero;
			}

			base.OnHandleDestroyed(e);
		}

		/// <summary>
		/// Update display of the executable
		/// </summary>
		protected override void OnResize(EventArgs e)
		{
			if (this.appHwnd != IntPtr.Zero)
                NativeMethods.MoveWindow(appHwnd, 0, 0, this.Width, this.Height, true);

            base.OnResize(e);
		}
	}
}