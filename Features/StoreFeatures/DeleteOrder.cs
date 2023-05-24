using MeDirectApiProject.Helpers;
using MeDirectApiProject.TestScenarios;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using RestSharp;

namespace MeDirectApiProject.StoreFeatures
{
    // NUnit Allure attributes for reporting
    [AllureNUnit]
    [AllureEpic("Store")]
    [AllureFeature("Delete Order Feature")]
    [AllureSuite("Store Suite")]
    public class DeleteOrder : BaseTest
    {
        private Order order;

        [SetUp]
        public void Setup()
        {
            // Generate order data and create the order
            order = OrderGenerator.GenerateOrderData();
            wrapper.ExecuteRequest(endpoints["Store"]["Order"], Method.POST, order);
        }

        [Test(Description = "Successfully Delete Order")]
        [AllureTag("Positive")]
        public void DeleteOrder_Successfully()
        {
            //delete the order
            var response = wrapper.ExecuteRequest($"{endpoints["Store"]["Order"]}{order.id}", Method.DELETE);
            JObject jsonResponse = JObject.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(200), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 200");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That(jsonResponse["message"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((int)response.StatusCode, Is.EqualTo(200), $"Response status code should be 200. Actual code: {(int)response.StatusCode}");
            });
        }

        [Test(Description = "Delete Non-exist Order")]
        [AllureTag("Negative")]
        public void DeleteOrder_NoExistOrder()
        {
            // Delete the order first to ensure it doesn't exist
            wrapper.ExecuteRequest($"{endpoints["Store"]["Order"]}{order.id}", Method.DELETE);

            // Send a DELETE request to delete a non-existing order
            var response = wrapper.ExecuteRequest($"{endpoints["Store"]["Order"]}{order.id}", Method.DELETE);
            JObject jsonResponse = JObject.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(404), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 404");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That(jsonResponse["message"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["message"], Is.EqualTo("Order Not Found"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: Order Not Found");
                Assert.That((int)response.StatusCode, Is.EqualTo(404), $"Response status code should be 404. Actual code: {(int)response.StatusCode}");
            });
        }
    }
}
