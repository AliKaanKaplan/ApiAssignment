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
    [AllureFeature("Login User Feature")]
    [AllureSuite("User Suite")]
    public class LoginUser : BaseTest
    {
        private User user;

        [SetUp]
        public void Setup()
        {
            // Generate fake user data
            user = UserGenerator.GenerateUserData();
        }

        [Test(Description = "Successful login")]
        [AllureTag("Positive")]
        public void Login_Successful()
        {
            // Create the user using the wrapper
            wrapper.ExecuteRequest(endpoints["User"]["User"], Method.POST, user);

            // Perform login request with username and password
            var response = wrapper.ExecuteRequest(endpoints["User"]["Login"], Method.GET, queryParamName: user.firstName, queryParamValue: user.password);
            JObject jsonResponse = JObject.Parse(response.Content);

            // Extract session key from the response message
            int separatorIndex = ((string)jsonResponse["message"]).IndexOf(":");
            string sessionKey = ((string)jsonResponse["message"]).Substring(separatorIndex + 1).Trim();

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(200), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 200");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That(jsonResponse["message"].Type, Is.EqualTo(JTokenType.String), "Message should be of type string");
                Assert.That((string)jsonResponse["message"], Is.EqualTo($"logged in user session:{sessionKey}"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: something bad happened");
                Assert.That((int)response.StatusCode, Is.EqualTo(200), $"Response status code should be 200. Actual code: {(int)response.StatusCode}");
            });
        }

        [Test(Description = "Login with wrong username")]
        [AllureTag("Negative")]
        public void Login_WithWrongUsername()
        {
            // Attempt to login with wrong username
            var response = wrapper.ExecuteRequest(endpoints["User"]["Login"], Method.GET, queryParamName: "WrongUsername", queryParamValue: user.password);

            // Assert that the response status code is 400 (Bad Request)
            Assert.That((int)response.StatusCode, Is.EqualTo(400), $"Response status code should be 400. Actual code: {(int)response.StatusCode}");
        }

        [Test(Description = "Login with wrong password")]
        [AllureTag("Negative")]
        public void Login_WithWrongPassword()
        {
            // Attempt to login with wrong password
            var response = wrapper.ExecuteRequest(endpoints["User"]["Login"], Method.GET, queryParamName: user.username, queryParamValue: "WrongPassword");

            // Assert that the response status code is 400 (Bad Request)
            Assert.That((int)response.StatusCode, Is.EqualTo(400), $"Response status code should be 400. Actual code: {(int)response.StatusCode}");
        }
    }
}
