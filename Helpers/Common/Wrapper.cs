using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Internal;
using RestSharp;
using System.Reflection;

namespace MeDirectApiProject.Helpers
{
    public class Wrapper
    {
        private RestClient client;
        private RequestLogger logger;

        public Wrapper(string baseUrl)
        {
            client = new RestClient(baseUrl);
            logger = new RequestLogger();
        }

        // Executes a REST request with the specified parameters and returns the response.
        public IRestResponse ExecuteRequest(JToken path, Method method, object body = null, string urlSegment = null, string queryParamName = null, string queryParamValue = null, string bearerToken = null, string contentType = "application/json")
        {
            RestRequest request = new RestRequest(path.ToString(), method);

            if (body != null)
            {
                // Adds the request body based on the content type.
                if (contentType == "application/json")
                {
                    request.AddJsonBody(body);
                }
                else if (contentType == "multipart/form-data")
                {
                    PropertyInfo[] properties = body.GetType().GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        string name = property.Name;
                        object value = property.GetValue(body);
                        request.AddParameter(name, value);
                    }
                }
            }

            // Adds URL segment and query parameters if provided.
            if (urlSegment != null)
                request.AddUrlSegment(urlSegment, queryParamValue);

            if (queryParamName != null && queryParamValue != null)
                request.AddQueryParameter(queryParamName, queryParamValue);

            // Adds authorization header if a bearer token is provided.
            if (bearerToken != null)
                request.AddHeader("Authorization", "Bearer " + bearerToken);

            IRestResponse response = client.Execute(request);

            // Logs the request and response details.
            logger.LogRequest(request, response, method);
            logger.LogResponse(response);

            return response;
        }

        // Uploads an image for a pet with the specified ID.
        public IRestResponse UploadImage(int petId, string imagePath, Method method)
        {
            RestRequest request = new RestRequest($"pet/{petId}/uploadImage", method);
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddParameter("additionalMetadata", "QaTest");
            request.AddFile("file", File.ReadAllBytes(imagePath), Path.GetFileName(imagePath), "image/png");

            IRestResponse response = client.Execute(request);

            // Logs the request and response details.
            logger.LogRequest(request, response, method);
            logger.LogResponse(response);

            return response;
        }

        public Dictionary<string, string> GetHeaders(string key)
        {

            var headers = new Dictionary<string, string>
            {
                { "accept", "application/json" },
                { "api_key", key }
            };

            return headers;
        }

    }
}
