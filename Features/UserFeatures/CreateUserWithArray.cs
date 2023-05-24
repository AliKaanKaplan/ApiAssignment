using MeDirectApiProject.Helpers;
using MeDirectApiProject.TestScenarios;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using RestSharp;
using System.Net;

namespace MeDirectApiProject.UserFeatures
{
    // NUnit Allure attributes for reporting
    [AllureNUnit]
    [AllureEpic("User")]
    [AllureFeature("Create User Feature")]
    [AllureSuite("User Suite")]
    public class CreateUserWithArray : BaseTest
    {
        private User user;
        private List<User> requestBodyArray;

        [SetUp]
        public void Setup()
        {
            // Generate fake user data
            user = UserGenerator.GenerateUserData();
            requestBodyArray = new List<User> { user };
        }

        [Test(Description = "Create User with Array List and List")]
        [TestCaseSource(nameof(GetEndpointPath), new object[] { "User", "CreateUserWithArray" })]
        [TestCaseSource(nameof(GetEndpointPath), new object[] { "User", "CreateUserWithList" })]
        [AllureTag("Positive")]
        public void CreateUserWithArrayAndList(string path)
        {
            // Create the user with array and list data and retrieve the response
            var response = wrapper.ExecuteRequest(path, Method.POST, requestBodyArray);
            JObject jsonResponse = JObject.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(200), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 200");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That(jsonResponse["message"].Type, Is.EqualTo(JTokenType.String), "Message should be of type string");
                Assert.That((string)jsonResponse["message"], Is.EqualTo("ok"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: ok");
                Assert.That((int)response.StatusCode, Is.EqualTo(200), $"Response status code should be 200. Actual code: {(int)response.StatusCode}");
            });
        }

        [Test(Description = "Create User with Array List and List, Wrong method type")]
        [TestCaseSource(nameof(GetEndpointPath), new object[] { "User", "CreateUserWithArray" })]
        [TestCaseSource(nameof(GetEndpointPath), new object[] { "User", "CreateUserWithList" })]
        [AllureTag("Negative")]
        public void CreateUserWithArrayAndList_WrongMethodType(string path)
        {
            // Attempt to create the user with array and list data using the wrong method type and retrieve the response
            var response = wrapper.ExecuteRequest(path, Method.PUT, requestBodyArray);
            JObject jsonResponse = JObject.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(405), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 200");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That((int)response.StatusCode, Is.EqualTo(405), $"Response status code should be 405. Actual code: {(int)response.StatusCode}");
            });
        }

        [Test(Description = "Create User with Array List and List, With Invalid ID")]
        [TestCaseSource(nameof(GetEndpointPath), new object[] { "User", "CreateUserWithArray" })]
        [TestCaseSource(nameof(GetEndpointPath), new object[] { "User", "CreateUserWithList" })]
        [AllureTag("Negative")]
        public void CreateUserWithArrayAndList_WithInvalidId(string path)
        {
            // Update the user ID with an invalid value
            User userToUpdate = requestBodyArray[0] as User;
            userToUpdate.id = "InvalidId";

            // Attempt to create the user with array and list data and retrieve the response
            var response = wrapper.ExecuteRequest(path, Method.POST, requestBodyArray);
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

        [Test(Description = "Create User with Array List and List, With Invalid Status")]
        [TestCaseSource(nameof(GetEndpointPath), new object[] { "User", "CreateUserWithArray" })]
        [TestCaseSource(nameof(GetEndpointPath), new object[] { "User", "CreateUserWithList" })]
        [AllureTag("Negative")]
        public void CreateUserWithArrayAndList_WithInvalidStatus(string path)
        {
            // Update the user status with an invalid value
            User userToUpdate = requestBodyArray[0] as User;
            userToUpdate.userStatus = "InvalidStatus";

            // Attempt to create the user with array and list data and retrieve the response
            var response = wrapper.ExecuteRequest(path, Method.POST, user);
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
