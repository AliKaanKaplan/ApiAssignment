using MeDirectApiProject.Helpers;
using Newtonsoft.Json.Linq;

namespace MeDirectApiProject.TestScenarios
{
    // Base class for test scenarios
    public class BaseTest
    {
        // Wrapper instance for API requests
        protected static Wrapper wrapper;

        // Flag to track if allure reports have been cleaned
        protected static bool allureReportsCleaned = false;

        // JSON object to store endpoints
        protected static JObject endpoints;

        // Setup method executed before each test
        [SetUp]
        public void SetUp()
        {
            // Create a new instance of the wrapper with the API base URL
            wrapper = new Wrapper("https://petstore.swagger.io/v2");

            // Clear allure reports folder if not already cleaned
            if (!allureReportsCleaned)
            {
                AllureHelper.ClearAllureReportsFolder();
                LoadEndpointsFromJson();
                allureReportsCleaned = true;
            }
        }

        // Load endpoints from JSON file
        private void LoadEndpointsFromJson()
        {
            string json = System.IO.File.ReadAllText(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "..\\Utils\\Endpoints.json"));
            endpoints = JObject.Parse(json);
        }

        // Get the endpoint path from the JSON based on the domain and path parameters
        public static IEnumerable<string> GetEndpointPath(string domain, string path)
        {

            string json = System.IO.File.ReadAllText(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "..\\Utils\\Endpoints.json"));
            var endpointsJson = JObject.Parse(json);

            yield return (string)endpointsJson[domain][path];
        }
    }
}
