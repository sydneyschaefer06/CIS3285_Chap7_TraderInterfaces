using System;
using System.Globalization;
using System.IO;
using System.Xml.Linq;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class ConsoleLogger : ILogger
{
    private const string LogFilePath = "log.xml";
    private static readonly object FileLock = new();

    public void LogMessage(string message, params object[] args)
    {
        var type = "INFO";
        var separatorIndex = message.IndexOf(": ", StringComparison.Ordinal);

        if (separatorIndex > 0)
        {
            var prefix = message[..separatorIndex];
            if (prefix is "INFO" or "WARN" or "ERROR")
            {
                type = prefix;
                message = message[(separatorIndex + 2)..];
            }
        }

        LogMessage(type, message, args);
    }

    public void LogMessage(string type, string message, params object[] args)
    {
        var formattedMessage = string.Format(CultureInfo.CurrentCulture, message, args);
        Console.WriteLine("{0}: {1}", type, formattedMessage);

        lock (FileLock)
        {
            var document = File.Exists(LogFilePath)
                ? XDocument.Load(LogFilePath)
                : new XDocument(new XElement("logs"));

            if (document.Root?.Name != "logs")
            {
                throw new InvalidDataException($"The XML log file must have a <logs> root element: {LogFilePath}");
            }

            document.Root.Add(
                new XElement("log",
                    new XElement("type", type),
                    new XElement("message", formattedMessage)));
            document.Save(LogFilePath);
        }
    }
}
