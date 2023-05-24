using MeDirectApiProject.Helpers;
using MeDirectApiProject.TestScenarios;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using RestSharp;

namespace MeDirectApiProject.UserFeatures
{
    // NUnit Allure attributes for reporting
    [AllureNUnit]
    [AllureEpic("User")]
    [AllureFeature("Create User Feature")]
    [AllureSuite("User Suite")]

    public class CreateUser : BaseTest
    {
        private User user;

        [SetUp]
        public void Setup()
        {
            // Generate fake user data
            user = UserGenerator.GenerateUserData();
        }

        [Test(Description = "Successfully Create User")]
        [AllureTag("Positive")]
        public void CreateUser_Successfully()
        {
            // Create the user and retrieve the response
            var response = wrapper.ExecuteRequest(endpoints["User"]["User"], Method.POST, user);
            JObject jsonResponse = JObject.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(200), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 200");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That(jsonResponse["message"].Type, Is.EqualTo(JTokenType.String), "Message should be of type string");
                Assert.That((int)response.StatusCode, Is.EqualTo(200), $"Response status code should be 200. Actual code: {(int)response.StatusCode}");
            });
        }

        [Test(Description = "Create User With Wrong Id")]
        [AllureTag("Negative")]
        public void CreateUser_WithInvalidId()
        {
            // Set the user ID to an invalid value
            user.id = "WrongId";

            // Attempt to create the user and retrieve the response
            var response = wrapper.ExecuteRequest(endpoints["User"]["User"], Method.POST, user);
            JObject jsonResponse = JObject.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(500), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 500");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That(jsonResponse["message"].Type, Is.EqualTo(JTokenType.String), "Message should be of type string");
                Assert.That((string)jsonResponse["message"], Is.EqualTo("something bad happened"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: something bad happened");
                Assert.That((int)response.StatusCode, Is.EqualTo(500), $"Response status code should be 500. Actual code: {(int)response.StatusCode}");
            });
        }

        [Test(Description = "Create User With Invalid Status")]
        [AllureTag("Negative")]
        public void CreateUser_WithInvalidStatus()
        {
            // Set the user status to an invalid value
            user.userStatus = "InvalidStatus";

            // Attempt to create the user and retrieve the response
            var response = wrapper.ExecuteRequest(endpoints["User"]["User"], Method.POST, user);
            JObject jsonResponse = JObject.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(500), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 500");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That(jsonResponse["message"].Type, Is.EqualTo(JTokenType.String), "Message should be of type string");
                Assert.That((string)jsonResponse["message"], Is.EqualTo("something bad happened"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: something bad happened");
                Assert.That((int)response.StatusCode, Is.EqualTo(500), $"Response status code should be 500. Actual code: {(int)response.StatusCode}");
            });
        }
    }
}
