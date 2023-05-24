using Newtonsoft.Json;
using RestSharp;

namespace MeDirectApiProject.Helpers
{
    public interface IRequestLogger
    {
        void LogRequest(IRestRequest request, IRestResponse response, Method method);
        void LogResponse(IRestResponse response);
    }
    public class RequestLogger : IRequestLogger
    {
        public void LogRequest(IRestRequest request, IRestResponse response, Method method)
        {
            TestContext.WriteLine("--------------------------------------------");
            TestContext.WriteLine(response.ResponseUri.ToString());
            TestContext.WriteLine("Method Type : " + method.ToString());

            // Writes the request headers.
            foreach (var header in request.Parameters.Where(p => p.Type == ParameterType.HttpHeader))
            {
                TestContext.WriteLine($"{header.Name}: {header.Value}");
            }

            // Writes the request body if it exists and the method is not GET.
            if (method != Method.GET)
            {
                TestContext.WriteLine("Request Body:");
                foreach (var parameter in request.Parameters)
                {
                    if (parameter.Type == ParameterType.RequestBody)
                    {
                        TestContext.WriteLine(JsonConvert.SerializeObject(parameter.Value));
                        break;
                    }
                }
            }
        }

        public void LogResponse(IRestResponse response)
        {
            TestContext.Write("Response Status Code:");
            TestContext.WriteLine((int)response.StatusCode);

            TestContext.WriteLine("Response Content:");
            TestContext.WriteLine(response.Content);
        }
    }
}
