using System;
using System.Diagnostics;

namespace TheDevStop.StudioBash
{
    /// <summary>
    /// Application Display Control
    /// </summary>
    public class ApplicationControl : System.Windows.Forms.Panel
	{
		public event EventHandler ApplicationExited;

		/// <summary>
		/// Track if the application has been created
		/// </summary>
		bool created = false;

		/// <summary>
		/// Handle to the application Window
		/// </summary>
		IntPtr appWin;

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
			if (created == false)
			{
				// Mark that control is created
				created = true;

				// Initialize handle value to invalid
				appWin = IntPtr.Zero;

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
					appWin = p.MainWindowHandle;
				}
				catch (Exception ex)
				{
					//MessageBox.Show(this, ex.Message, "Error");
                    created = false;
					return;
				}

                var style = NativeMethods.GetWindowLong(appWin, NativeMethods.GWL_EXSTYLE);
                NativeMethods.SetWindowLong(appWin, NativeMethods.GWL_EXSTYLE, style ^ NativeMethods.WS_EX_LAYERED);

				// Put it into this form
                NativeMethods.SetParent(appWin, this.Handle);

				// Remove border and whatnot
                NativeMethods.SetWindowLong(appWin, NativeMethods.GWL_STYLE, NativeMethods.WS_VISIBLE);

				// Move the window to overlay it on this window
                NativeMethods.MoveWindow(appWin, 0, 0, this.Width, this.Height, true);
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
			if (appWin != IntPtr.Zero)
			{
				// Post a colse message
                NativeMethods.PostMessage(appWin, NativeMethods.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

				// Delay for it to get the message
				System.Threading.Thread.Sleep(1000);

				// Clear internal handle
				appWin = IntPtr.Zero;
			}

			base.OnHandleDestroyed(e);
		}

		/// <summary>
		/// Update display of the executable
		/// </summary>
		protected override void OnResize(EventArgs e)
		{
			if (this.appWin != IntPtr.Zero)
                NativeMethods.MoveWindow(appWin, 0, 0, this.Width, this.Height, true);

            base.OnResize(e);
		}
	}
}