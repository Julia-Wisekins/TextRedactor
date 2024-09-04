# Summary of the C# ASP.NET Application: Redaction Service
This C# ASP.NET standalone application, created for a technical test, implements a simple web service for text redaction. The application's core functionality is as follows:

**Web Service Endpoint**: The service provides a single endpoint available at /redact.

**POST Request**:
- Accepts arbitrary text in the request body.
- Returns the received text with specific words replaced by 'REDACTED'. These words are defined in a configurable list.
- Logs each POST request to a log file, including a timestamp and the original text before redaction.

**GET Request**:
- Returns the string "Redaction Service" to identify the service.

## Configuration Options:
- The list of words to be redacted and the service port are configurable using the appsettings.json file.
- The configuration is implemented using the options design pattern, allowing the application to dynamically read settings from the appsettings.json file without requiring code changes.
- This application supports dynamic port configuration at startup. 

## Logging:
Every POST request is logged to a file, capturing both the request's timestamp and the original unredacted text.
