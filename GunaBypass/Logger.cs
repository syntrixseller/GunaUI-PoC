public enum LogLevel
{
    Info,
    Warn,
    Success,
    Error
}
public class Logger
{
    private readonly object _lock = new();
    public void Log(LogLevel level, string message)
    {
        lock (_lock)
        {
            var originalcolor = Console.ForegroundColor;
            switch (level)
            {
                case LogLevel.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogLevel.Warn:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogLevel.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogLevel.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
            Console.WriteLine($"         {DateTime.Now} - {level}: {message}");
            Console.ForegroundColor = originalcolor;
        }
    }
}