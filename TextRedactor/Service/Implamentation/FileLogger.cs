using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using TextRedactor.Model;

namespace TextRedactor.Service.Implamentation
{
    public sealed class FileLoggerConfiguration
    {
        public int EventId { get; set; }
        public LogLevel MinimumLogLevel { get; set; }
    }

    /// <summary>
    /// Customized ILogger, writes logs to text files
    /// 
    /// This is a basic implementation to show it working in a production system this would
    /// be changed for a better logging system like SeriLog allowing for much more scalable
    /// logs
    /// </summary>
    public sealed class FileLogger(
    string name,
    Func<FileLoggerConfiguration> getCurrentConfig, StreamWriter logFileWriter) : ILogger
    {
        private readonly string _name = name;
        private readonly StreamWriter _logFileWriter = logFileWriter;
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel) =>
            getCurrentConfig().MinimumLogLevel <= logLevel;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            // Ensure that only information level and higher logs are recorded
            if (!IsEnabled(logLevel))
            {
                return;
            }

            // Get the formatted log message
            var message = formatter(state, exception);

            //Write log messages to text file
            _logFileWriter.WriteLine($"[{DateTime.UtcNow}] [{logLevel}] [{_name}] {message}");
            _logFileWriter.Flush();
        }
    }

    public sealed class FileLoggerProvider : ILoggerProvider
    {
        private readonly IDisposable? _onChangeToken;
        private FileLoggerConfiguration _currentConfig;
        private readonly ConcurrentDictionary<string, FileLogger> _loggers =
            new(StringComparer.OrdinalIgnoreCase);
        private readonly StreamWriter _logFileWriter;

        public FileLoggerProvider(
            IOptionsMonitor<FileLoggerConfiguration> config)
        {
            _currentConfig = config.CurrentValue;
            _onChangeToken = config.OnChange(updatedConfig => _currentConfig = updatedConfig);
            _logFileWriter = new(ConfigurationDetails.LoggingLocation, append: true);
        }

        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, name => new FileLogger(name, GetCurrentConfig, _logFileWriter));

        private FileLoggerConfiguration GetCurrentConfig() => _currentConfig;

        public void Dispose()
        {
            _logFileWriter?.Dispose();
            _loggers.Clear();
            _onChangeToken?.Dispose();
        }
    }
}
