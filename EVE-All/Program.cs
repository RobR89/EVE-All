using System;
using System.Threading;
using System.Windows.Forms;
using static EVE_All.EVEAllMain;

namespace EVE_All
{
    static class Program
    {
        private static Mutex singletonMutex = new Mutex(true, "EVE-All-App-Singleton");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Use a named singletonMutex to prevent multiple instances of the application running at once.
            if (singletonMutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new EVEAllMain());
                singletonMutex.ReleaseMutex();
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                NativeMethods.PostMessage(
                    (IntPtr)NativeMethods.HWND_BROADCAST,
                    NativeMethods.WM_SHOW_EVE_ALL,
                    IntPtr.Zero,
                    IntPtr.Zero);
            }
        }

    }
}
