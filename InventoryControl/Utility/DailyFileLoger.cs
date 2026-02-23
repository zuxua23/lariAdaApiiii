namespace InventoryControl.Utility;

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

public static class DailyFileLogger
{
    private static readonly object _lock = new object();
    private static string _logDirectory =
        "C:\\Users\\User\\source\\repos\\InventoryControl\\InventoryControl\\Logs\\";

    public static void SetLogDirectory(string path)
    {
        _logDirectory = path;
    }

    public static void Info(string message)
    {
        Write("INFO", message);
    }

    public static void Warn(string message)
    {
        Write("WARN", message);
    }

    public static void Error(string message, Exception? ex = null)
    {
        var fullMessage = ex == null
            ? message
            : $"{message}{Environment.NewLine}{ex}";

        Write("ERROR", fullMessage);
    }

    private static void Write(string level, string message)
    {
        try
        {
            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
            CleanOldLogs(30);

            var date = DateTime.Now.ToString("yyyy-MM-dd");
            var filePath = Path.Combine(_logDirectory, $"app-{date}.log");

            // Ambil info caller (skip Write + Info/Warn/Error)
            var stackTrace = new StackTrace(true);
            var frame = stackTrace.GetFrame(2);

            var fileName = frame?.GetFileName();
            var lineNumber = frame?.GetFileLineNumber();
            var methodName = frame?.GetMethod()?.Name;

            var sourceInfo = fileName == null
                ? "[unknown]"
                : $"{Path.GetFileName(fileName)}:{lineNumber} ({methodName})";

            var logLine = new StringBuilder()
                .Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                .Append(" [").Append(level).Append("] ")
                .Append(sourceInfo)
                .Append(" - ")
                .Append(message)
                .AppendLine()
                .ToString();

            lock (_lock)
            {
                File.AppendAllText(filePath, logLine);
            }
        }
        catch
        {
            // Logging tidak boleh menjatuhkan aplikasi
        }
    }

    private static void CleanOldLogs(int days)
    {
        try
        {
            var dirInfo = new DirectoryInfo(_logDirectory);
            if (!dirInfo.Exists) return;

            var oldFiles = dirInfo.GetFiles("app-*.log");
            foreach (var file in oldFiles)
            {
                if (file.CreationTime < DateTime.Now.AddDays(-days))
                {
                    try { file.Delete(); } catch { /* ignore */ }
                }
            }
        }
        catch
        {
            // Jangan lempar exception
        }
    }
}