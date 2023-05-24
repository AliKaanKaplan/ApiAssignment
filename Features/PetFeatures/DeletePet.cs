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
    [AllureFeature("Delete pet Feature")]
    [AllureSuite("Pet Suite")]
    internal class DeletePet : BaseTest
    {
        private Pet pet;

        [SetUp]
        public void Setup()
        {
            // Generate pet data
            pet = PetGenerator.GeneratePetData();

            // Create a new pet 
            wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.POST, pet);
        }

        [Test(Description = "Successfully Deleting A Pet")]
        [AllureTag("Positive")]
        public void DeletePet_Successfully()
        {
            var headers = wrapper.GetHeaders("special-key");

            var response = wrapper.ExecuteRequest($"{endpoints["Pet"]["Pet"]}{pet.id}", Method.DELETE, headers);
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

        [Test(Description = "Deleting A Pet with Invalid api key")]
        [AllureTag("Negative")]
        public void DeletePet_WithInvalidApiKey()
        {
            var headers = wrapper.GetHeaders("invalid -key");

            var response = wrapper.ExecuteRequest($"{endpoints["Pet"]["Pet"]}{pet.id}", Method.DELETE, headers);
            Assert.That((int)response.StatusCode, Is.EqualTo(401), $"Response status code should be 401. Actual code: {(int)response.StatusCode}");
        }

        [Test(Description = "Deleting a non-exist Pet")]
        [AllureTag("Negative")]
        public void DeletePet_NoExist()
        {
            var headers = wrapper.GetHeaders("special-key");

            wrapper.ExecuteRequest($"{endpoints["Pet"]["Pet"]}{pet.id}", Method.DELETE, headers);
            var response = wrapper.ExecuteRequest($"/pet/{pet.id}", Method.DELETE, headers);
            Assert.That((int)response.StatusCode, Is.EqualTo(404), $"Response status code should be 404. Actual code: {(int)response.StatusCode}");
        }
    }
}
