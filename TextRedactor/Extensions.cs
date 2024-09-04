using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using System.Text;
using TextRedactor.Service.Implamentation;

namespace TextRedactor
{
    public static class RequestExtensions
    {
        /// <summary>
        /// gets the body information
        /// </summary>
        /// <param name="request">the user request</param>
        /// <param name="encoding">how the bytes are converted to text (basic character encoding)</param>
        /// <returns></returns>
        public static async Task<string> GetStringBodyAsync(
            this HttpRequest request,
            Encoding? encoding = null)
        {
            if (!request.Body.CanSeek)
            {
                // We only do this if the stream isn't *already* seekable,
                // as EnableBuffering will create a new stream instance
                // each time it's called
                request.EnableBuffering();
            }

            request.Body.Position = 0;

            var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8); // UTF-8 is a variable-length character encoding standard used for electronic communication and is used by most web apps
            var body = await reader.ReadToEndAsync().ConfigureAwait(false);

            //resetting position
            request.Body.Position = 0;

            return body;
        }
    }

    public static class FileLoggerLoggerExtensions
    {
        /// <summary>
        /// adds the <see cref="ILoggerProvider"/> to the services
        /// </summary>
        /// <param name="builder"></param>
        /// <returns>builder with the logger</returns>
        public static ILoggingBuilder AddFileLogger(
            this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, FileLoggerProvider>());

            LoggerProviderOptions.RegisterProviderOptions
                <FileLoggerConfiguration, FileLoggerProvider>(builder.Services);

            return builder;
        }
    }
}
