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
    [AllureFeature("Update User Feature")]
    [AllureSuite("User Suite")]

    public class UpdateUser : BaseTest
    {
        private User user, newUser;

        [SetUp]
        public void Setup()
        {
            // Generate fake user data for existing and new users
            user = UserGenerator.GenerateUserData();
            newUser = UserGenerator.GenerateUserData();
        }

        [Test(Description = "Updating an existing user")]
        [AllureTag("Positive")]
        public void UpdateUser_ExistUser()
        {
            // Create the existing user
            wrapper.ExecuteRequest(endpoints["User"]["User"], Method.POST, user);

            // Update the existing user with new user data
            var response = wrapper.ExecuteRequest(endpoints["User"]["User"] + user.username, Method.PUT, newUser);
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


        [Test(Description = "updating a non-existent user")]
        [AllureTag("Negative")]
        public void UpdateUser_NonExistUser()
        {
            // Create the existing user 
            wrapper.ExecuteRequest(endpoints["User"]["User"], Method.POST, user);

            // Delete the existing user
            wrapper.ExecuteRequest(endpoints["User"]["User"] + user.username, Method.DELETE);

            // Attempt to update user which was deleted
            var response = wrapper.ExecuteRequest(endpoints["User"]["User"] + user.username, Method.PUT, newUser);

            Assert.That((int)response.StatusCode, Is.EqualTo(404), $"Response status code should be 404. Actual code: {(int)response.StatusCode}");
        }

    }
}
