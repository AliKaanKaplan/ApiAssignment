using MeDirectApiProject.Helpers;
using MeDirectApiProject.TestScenarios;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using RestSharp;

namespace MeDirectApiProject.UserFeatures
{
    // NUnit Allure attributes for reporting
    [AllureNUnit]
    [AllureEpic("User")]
    [AllureFeature("Logout User Feature")]
    [AllureSuite("User Suite")]
    public class LogoutUser : BaseTest
    {
        private User user;
        private UserHelper userHelper;

        [SetUp]
        public void Setup()
        {
            // Generate fake user data and create an instance of UserHelper
            user = UserGenerator.GenerateUserData();
            userHelper = new UserHelper();
        }

        [Test(Description = "Successfully logout")]
        [AllureTag("Positive")]
        public void Logout_Successfully()
        {
            // Create the user 
            wrapper.ExecuteRequest(endpoints["User"]["User"], Method.POST, user);

            // Get the bearer token
            string bearerToken = userHelper.getToken(user.firstName, user.password);

            // Execute the logout request with the bearer token
            var response = wrapper.ExecuteRequest(endpoints["User"]["Logout"], Method.GET, bearerToken: bearerToken);

            Assert.That((int)response.StatusCode, Is.EqualTo(200), $"Response status code should be 200. Actual code: {(int)response.StatusCode}");
        }

        [Test(Description = "Logout test without token")]
        [AllureTag("Negative")]
        public void Logout_WithoutToken()
        {
            // Attempt to logout without providing a bearer token
            var response = wrapper.ExecuteRequest(endpoints["User"]["Logout"], Method.GET, queryParamName: "WrongUsername", queryParamValue: user.password);

            // Assert that the response status code is 401 (Unauthorized)
            Assert.That((int)response.StatusCode, Is.EqualTo(401), $"Response status code should be 400. Actual code: {(int)response.StatusCode}");
        }

        [Test(Description = "Logout test with wrong method type")]
        [AllureTag("Negative")]
        public void Logout_WrongMethodType()
        {
            // Attempt to logout using the wrong method type (POST instead of GET)
            var response = wrapper.ExecuteRequest(endpoints["User"]["Logout"], Method.POST, queryParamName: user.firstName, queryParamValue: user.password);

            // Assert that the response status code is 405 (Method Not Allowed)
            Assert.That((int)response.StatusCode, Is.EqualTo(405), $"Response status code should be 400. Actual code: {(int)response.StatusCode}");
        }
    }
}
