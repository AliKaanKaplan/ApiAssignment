using MeDirectApiProject.Helpers;
using MeDirectApiProject.TestScenarios;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using RestSharp;

namespace MeDirectApiProject.StoreFeatures
{
    // NUnit Allure attributes for reporting
    [AllureNUnit]
    [AllureEpic("Store")]
    [AllureFeature("Create Order Feature")]
    [AllureSuite("Store Suite")]
    public class CreateOrder : BaseTest
    {
        private Order order;

        [SetUp]
        public void Setup()
        {
            order = OrderGenerator.GenerateOrderData();
        }

        [Test(Description = "Successfully Create Order")]
        [AllureTag("Positive")]
        public void CreateOrder_Successfully()
        {
            // create an order
            var response = wrapper.ExecuteRequest(endpoints["Store"]["Order"], Method.POST, order);

            Assert.That(response.Content, Is.EqualTo(JsonConvert.SerializeObject(order)));
        }

        [Test(Description = "Create Order with wrong method type")]
        [AllureTag("Negative")]
        public void CreateOrder_wrongMethodType()
        {
            // Send a PUT request instead of POST to create an order
            var response = wrapper.ExecuteRequest(endpoints["Store"]["Order"], Method.PUT, order);

            // Parse the response content as a JSON object
            JObject jsonResponse = JObject.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(405), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 405");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That((int)response.StatusCode, Is.EqualTo(405), $"Response status code should be 405. Actual code: {(int)response.StatusCode}");
            });
        }
    }
}
