
using System.Text.RegularExpressions;

namespace TextRedactor.Service.Implamentation
{
    /// <summary>
    /// a redaction interface to allow for easy modification in future, easier testing options (mocking, TestFixture etc), and dependancy injection. 
    /// </summary>
    /// <param name="redactionList"> list of redacted words</param>
    public class RedactorUsingRegex(IEnumerable<string> redactionList) : IRedactor
    {
        private readonly IEnumerable<string> _redactionList = redactionList;

        /// <summary>
        /// Takes a string input and returns an upated string with the new redactions
        /// </summary>
        /// <param name="textToRedact">string without redactions</param>
        /// <returns>string with redactions</returns>
        /// <exception cref="ConfigurationException">configuration error while attempting to locate the List of redacted words</exception>
        public string RedactText(string textToRedact)
        {
            if(_redactionList == null)
            {
                //example of a custom exception, null-ref would have also worked.
                throw new ConfigurationException("Unable to load the redactions list");
            }

            //Using Regex as an easy way to enforce word boundries
            foreach (var item in _redactionList)
            {
                textToRedact = Regex.Replace(textToRedact, @$"\b{item}\b", "REDACTED", RegexOptions.IgnoreCase);
            }

            return textToRedact;
        }
    }
}