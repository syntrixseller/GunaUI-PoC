Logger logger = new();
Console.Title = "Guna Bypasser by dex :3";
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine(
    @"
               GUNA BYPASSER :3
                              - By @syntrixdex
    ");
logger.Log(LogLevel.Info, " ~@ Process has been started...");
bool isdetected = false;
Task thread = Task.Run(() =>
{
    if (!isdetected)
    {
        WindowHider.HideGunaDialog();
        isdetected = true;
    }
});
Console.ReadKey();