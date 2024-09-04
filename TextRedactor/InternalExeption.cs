namespace TextRedactor

{/// <summary>
/// Custom Exception for issues in the configuration file
/// </summary>
/// <param name="message"></param>
    public class ConfigurationException(string message = "Error accessomg configuration details.") : Exception(message)
    {
    }
}
