using MeDirectApiProject.Helpers;
using MeDirectApiProject.TestScenarios;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using RestSharp;
using System.Text.Json;

namespace MeDirectApiProject.UserFeatures
{
    // NUnit Allure attributes for reporting
    [AllureNUnit]
    [AllureEpic("User")]
    [AllureFeature("Get User Feature")]
    [AllureSuite("User Suite")]
    public class GetUserByUsername : BaseTest
    {
        private User user;

        [SetUp]
        public void Setup()
        {
            // Generate fake user data
            user = UserGenerator.GenerateUserData();
        }

        [Test(Description = "Successfully Getting User by Username")]
        [AllureTag("Positive")]
        public void GetUserByUsername_Successfully()
        {
            // Create the user 
            wrapper.ExecuteRequest(endpoints["User"]["User"], Method.POST, user);

            // Retrieve the user by username
            var response = wrapper.ExecuteRequest(endpoints["User"]["User"] + user.username, Method.GET);

            // Assert that the response content matches the serialized user object
            Assert.That(response.Content, Is.EqualTo(JsonSerializer.Serialize(user)));

            // Assert that the response status code is 200 (OK)
            Assert.That((int)response.StatusCode, Is.EqualTo(200), $"Response status code should be 200. Actual code: {(int)response.StatusCode}");
        }

        [Test(Description = "Getting User by Username with wrong method type")]
        [AllureTag("Negative")]
        public void GetUserByUsername_WrongMethodType()
        {
            // Create the user using the wrapper
            wrapper.ExecuteRequest(endpoints["User"]["User"], Method.POST, user);

            // Attempt to retrieve the user by username using the wrong method type (POST instead of GET)
            var response = wrapper.ExecuteRequest(endpoints["User"]["User"] + user.username, Method.POST);

            // Parse the response content into a JObject
            JObject jsonResponse = JObject.Parse(response.Content);

            // Perform multiple assertions on the response data
            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(405), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 200");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That((int)response.StatusCode, Is.EqualTo(405), $"Response status code should be 405. Actual code: {(int)response.StatusCode}");
            });
        }

        [Test(Description = "Getting a non-existent user by username")]
        [AllureTag("Negative")]
        public void GetUserByUsername_NoExistUsername()
        {
            // Create the user using the wrapper
            wrapper.ExecuteRequest(endpoints["User"]["User"], Method.POST, user);

            // Delete the user to make it non-existent
            wrapper.ExecuteRequest(endpoints["User"]["User"] + user.username, Method.DELETE);

            // Attempt to retrieve the non-existent user by username
            var response = wrapper.ExecuteRequest(endpoints["User"]["User"] + user.username, Method.GET);

            // Parse the response content into a JObject
            JObject jsonResponse = JObject.Parse(response.Content);

            // Perform multiple assertions on the response data
            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(1), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 200");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("error"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That(jsonResponse["message"].Type, Is.EqualTo(JTokenType.String), "Message should be of type string");
                Assert.That((string)jsonResponse["message"], Is.EqualTo("User not found"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: User not found");
                Assert.That((int)response.StatusCode, Is.EqualTo(404), $"Response status code should be 404. Actual code: {(int)response.StatusCode}");
            });
        }
    }
}
