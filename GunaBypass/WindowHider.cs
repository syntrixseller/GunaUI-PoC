using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public class WindowHider
{
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    private const int SW_HIDE = 0;
    public static void HideGunaDialog()
    {
        Logger logger = new();
        bool hasloggedsuccess = false;
        bool hasloggedwarn = false;
        Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    EnumWindows((hWnd, lParam) =>
                    {
                        StringBuilder windowtext = new(256);
                        GetWindowText(hWnd, windowtext, windowtext.Capacity);
                        var proccesses = Process.GetProcesses();
                        foreach (var p in proccesses)
                        {
                            if (windowtext.ToString().Contains("Guna.UI2", StringComparison.OrdinalIgnoreCase) &&
                                p.ProcessName == "devenv")
                            {
                                ShowWindow(hWnd, SW_HIDE);
                                if (!hasloggedsuccess)
                                {
                                    logger.Log(LogLevel.Success, " ~@ Guna has been bypassed!");
                                    hasloggedsuccess = true;
                                }
                                return false;
                            }
                        }
                        return true;
                    }, IntPtr.Zero);
                    if (!hasloggedsuccess && !hasloggedwarn)
                    {
                        logger.Log(LogLevel.Warn, " ~@ Couldn't find Guna, Is Visual Studio Open with Guna Active?");
                        hasloggedwarn = true;
                    }
                }
                catch
                {
                }

                Thread.Sleep(1000);
            }
        });
    }
}
