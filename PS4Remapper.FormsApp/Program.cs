using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS4Remapper.FormsApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.ThreadException += new ThreadExceptionEventHandler(OnThreadException);

            // Set the unhandled exception mode to force all Windows Forms errors
            // to go through the handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ApplicationExit += ApplicationOnApplicationExit;

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(OnUnhandledException);

            Application.Run(new MainForm());
        }

        private static void ApplicationOnApplicationExit(object sender, EventArgs eventArgs)
        {
            Remapper.Instance.Stop();   
        }

        private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Remapper.Instance.Stop();
            MessageBox.Show(e.Exception.ToString(), "Unhandled Thread Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Remapper.Instance.Stop();
            MessageBox.Show(e.ExceptionObject.ToString(), "Unhandled Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
