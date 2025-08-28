---

# GunaUI Login Bypass PoC

A proof-of-concept (PoC) demonstrating how to programmatically hide the GunaUI login screen prompt in applications that use overly aggressive or unlicensed UI prompts.

> ⚠️ **Disclaimer**: This project is for **educational and research purposes only**. Use responsibly and only on software you own or have permission to modify.

## 📖 Overview

Some applications using GunaUI controls may implement intrusive or unskippable login or nag-screens that disrupt the user experience. This PoC uses a simple C# method to find and hide these windows without interacting with the prompt directly.

## 🛠️ Technique

The bypass works by:

- Enumerating all top-level windows
- Identifying windows with the class name `Guna.UI2.WinForms.Guna2MessageDialog`
- Using `ShowWindow` with `SW_HIDE` to hide the dialog

## 🧩 Code Example

```csharp
﻿using System.Diagnostics;
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
```

## 🚀 Usage

1. Clone the repository
2. Open the solution in Visual Studio
3. Build the project
4. Run the executable (or inject into target process)

## 📌 Requirements

- .NET Framework 8.0 or later
- Windows Operating System

## ⚠️ Legal & Ethical Note

This tool is intended for:
- Educational purposes
- Security research
- Testing software you own

Do not use to bypass licensing mechanisms illegally.

## 🔗 Related Resources

- [GunaUI Official Website](https://guna-ui.com/)
- [WinAPI ShowWindow Documentation](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-showwindow)

## 📜 License

This project is licensed under the MIT License – see the [LICENSE](LICENSE) file for details.

---
