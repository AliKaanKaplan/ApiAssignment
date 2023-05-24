using MeDirectApiProject.Helpers;
using MeDirectApiProject.TestScenarios;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using RestSharp;

namespace MeDirectApiProject.PetFeatures
{
    // NUnit Allure attributes for reporting
    [AllureNUnit]
    [AllureEpic("Pet")]
    [AllureFeature("Find pet Feature")]
    [AllureSuite("Pet Suite")]
    public class FindPetByStatus : BaseTest
    {
        private Pet pet;

        [SetUp]
        public void Setup()
        {
            // Generate pet data
            pet = PetGenerator.GeneratePetData();
        }

        [Test(Description = "Successfully Find A Pet by status")]
        [AllureTag("Positive")]
        public void FindPetByStatus_Successfully()
        {
            // Execute the request to find pets by status
            var response = wrapper.ExecuteRequest(endpoints["Pet"]["FindByStatus"], Method.GET, queryParamName: "status", queryParamValue: pet.status);

            Assert.That((int)response.StatusCode, Is.EqualTo(200), $"Response status code should be 200. Actual code: {(int)response.StatusCode}");
        }

        [Test(Description = "Find A Pet by status with wrong method type")]
        [AllureTag("Negative")]
        public void FindPetByStatus_WrongMethod()
        {
            // Execute the request to find pets by status using the wrong method type (POST)
            var response = wrapper.ExecuteRequest(endpoints["Pet"]["FindByStatus"], Method.POST, queryParamName: "status", queryParamValue: pet.status);
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

        [Test(Description = "Find A Pet by status with Invalid status")]
        [AllureTag("Negative")]
        public void FindPetByStatus_InvalidStatus()
        {
            // Execute the request to find pets by an invalid status
            var response = wrapper.ExecuteRequest(endpoints["Pet"]["FindByStatus"], Method.GET, queryParamName: "status", queryParamValue: "invalid_status");

            Assert.That((int)response.StatusCode, Is.EqualTo(400), $"Response status code should be 400. Actual code: {(int)response.StatusCode}");
        }
    }
}
