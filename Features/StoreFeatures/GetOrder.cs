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
    [AllureFeature("Get Order Feature")]
    [AllureSuite("Store Suite")]
    public class GetOrder : BaseTest
    {
        private Order order;

        [SetUp]
        public void Setup()
        {
            // Generate order data and create the order
            order = OrderGenerator.GenerateOrderData();
            wrapper.ExecuteRequest(endpoints["Store"]["Order"], Method.POST, order);
        }

        [Test(Description = "Getting Order with Wrong method type")]
        [AllureTag("Negative")]
        public void GetOrder_WrongMethodType()
        {
            // Send a GET request with the wrong method type
            var response = wrapper.ExecuteRequest($"{endpoints["Store"]["Order"]}{order.id}", Method.PUT);
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

        [Test(Description = "Getting Non-exist Order")]
        [AllureTag("Negative")]
        public void GetOrder_NoExistOrder()
        {
            // Delete the order first to ensure it doesn't exist
            wrapper.ExecuteRequest($"{endpoints["Store"]["Order"]}{order.id}", Method.DELETE);

            // Send a GET request to retrieve a non-existing order
            var response = wrapper.ExecuteRequest($"{endpoints["Store"]["Order"]}{order.id}", Method.GET);
            JObject jsonResponse = JObject.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(1), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 1");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("error"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: error");
                Assert.That(jsonResponse["message"].Type, Is.EqualTo(JTokenType.String), "Message should be of type string");
                Assert.That((string)jsonResponse["message"], Is.EqualTo("Order not found"), $"Actual message: {(string)jsonResponse["message"]}, Expected message: Order Not Found");
                Assert.That((int)response.StatusCode, Is.EqualTo(404), $"Response status code should be 404. Actual code: {(int)response.StatusCode}");
            });
        }

        [Test(Description = "Successfully Getting Order")]
        [AllureTag("Positive")]
        public void GetOrder_Successfully()
        {
            // Send a GET request to retrieve the order
            var response = wrapper.ExecuteRequest($"store/order/{order.id}", Method.GET);

            // Assert that the retrieved order matches the expected order
            Assert.That(JsonConvert.SerializeObject(order), Is.EqualTo(response.Content));
        }
    }
}
