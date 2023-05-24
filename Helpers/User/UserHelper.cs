using MeDirectApiProject.TestScenarios;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MeDirectApiProject.Helpers
{
    public class UserHelper : BaseTest
    {
        // Retrieves the authentication token for a given username and password.
        public string getToken(string username, string password)
        {

            var response = wrapper.ExecuteRequest(endpoints["User"]["Login"], Method.GET, queryParamName: username, queryParamValue: password);

            // Parses the response content as a JSON object.
            JObject jsonResponse = JObject.Parse(response.Content);

            // Extracts the token from the "message" property of the JSON response.
            int separatorIndex = ((string)jsonResponse["message"]).IndexOf(":");
            string token = ((string)jsonResponse["message"]).Substring(separatorIndex + 1).Trim();

          
            return token;
        }
    }
}
