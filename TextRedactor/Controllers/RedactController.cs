using Microsoft.AspNetCore.Mvc;
using TextRedactor.Service;

namespace TextRedactor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedactController(ILogger<RedactController> logger, IRedactor redactor) : ControllerBase
    {

        private readonly ILogger<RedactController> _logger = logger;
        private readonly IRedactor _redactor = redactor;

        [HttpGet(Name = "redact")]
        public string Get()
        {
            _logger.LogInformation("User retrieved information about the service with the Get request");
            return "Redaction Service";
        }

        [HttpPost(Name = "redact")]
        public async Task<ActionResult<string>> Post()
        {
            //uning an extension methon to make method less cluttered and more readable
            var body = await Request.GetStringBodyAsync();

            try
            {
                var bodyRedacted = _redactor.RedactText(body);

                //logging the request details to the consol and text file so that it can be found later
                var message = $"User requsted redaction service at {DateTime.UtcNow} with the Post request. Body: {body} Return Value: {bodyRedacted}";
                _logger.LogInformation(message);

                return bodyRedacted;
            }
            catch (Exception e)
            {
                //logging the error to the consol and text file so that it can be found later
                var message = e.ToString();
                _logger.LogError(message);

                //provide vague user error, they don't need to know whats wrong with the service
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to use the redaction service, Please try again later...");
            }


        }
    }
}
