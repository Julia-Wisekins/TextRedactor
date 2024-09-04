namespace TextRedactor.Model
{
    /// <summary>
    /// Information for configuration, so I didn't need to worry about syntax errors
    /// </summary>
    public static class ConfigurationDetails
    {
        /// <summary>
        /// Name of the logging file
        /// </summary>
        public static readonly string LoggingLocation = "console_log.txt";
        /// <summary>
        /// The Json object in <see href="file:///appsettings.json"></see> containing the list of Redacted Words
        /// </summary>
        public static readonly string RedactorConfigLocation = "RedactedText";
    }
}
