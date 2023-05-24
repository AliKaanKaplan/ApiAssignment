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
    [AllureFeature("Delete User Feature")]
    [AllureSuite("User Suite")]
    public class DeleteUser : BaseTest
    {
        private User user;

        // Setup method executed before each test
        [SetUp]
        public void Setup()
        {
            // Generate user data using UserGenerator
            user = UserGenerator.GenerateUserData();
        }


        [Test(Description = "Successfully Deleting User")]
        [AllureTag("Positive")]
        public void DeleteUser_Successfully()
        {
            // Create a user
            wrapper.ExecuteRequest(endpoints["User"]["User"], Method.POST, user);

            // Execute a DELETE request to delete the user
            var response = wrapper.ExecuteRequest(endpoints["User"]["User"] + user.username, Method.DELETE);

            // Parse the response content into a JObject
            JObject jsonResponse = JObject.Parse(response.Content);

            // Perform multiple assertions using Assert.Multiple
            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(200), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 200");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That(jsonResponse["message"].Type, Is.EqualTo(JTokenType.String), "Message should be of type string");
                Assert.That((string)jsonResponse["message"], Is.EqualTo(user.username), $"Actual type: {(string)jsonResponse["type"]}, Expected type: something bad happened");
                Assert.That((int)response.StatusCode, Is.EqualTo(200), $"Response status code should be 200. Actual code: {(int)response.StatusCode}");
            });
        }

   
        [Test(Description = "Deleting Non-Exist User")]
        [AllureTag("Negative")]
        public void DeleteUser_NoExistUser()
        {
            // create a user
            wrapper.ExecuteRequest(endpoints["User"]["User"], Method.POST, user);

            // Execute a DELETE request to delete the user
            wrapper.ExecuteRequest(endpoints["User"]["User"] + user.username, Method.DELETE);

            // Execute a DELETE request to delete the non-existing user
            var response = wrapper.ExecuteRequest(endpoints["User"]["User"] + user.username, Method.DELETE);

            // Verify that the response status code is 404
            Assert.That((int)response.StatusCode, Is.EqualTo(404), $"Response status code should be 404. Actual code: {(int)response.StatusCode}");
        }
    }
}
