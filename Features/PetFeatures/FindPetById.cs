using MeDirectApiProject.Helpers;
using MeDirectApiProject.TestScenarios;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using RestSharp;
using System.Text.Json;

namespace MeDirectApiProject.PetFeatures
{
    // NUnit Allure attributes for reporting
    [AllureNUnit]
    [AllureEpic("Pet")]
    [AllureFeature("Find pet Feature")]
    [AllureSuite("Pet Suite")]
    public class FindPetById : BaseTest
    {
        private Pet pet;

        [SetUp]
        public void Setup()
        {
            // Generate pet data
            pet = PetGenerator.GeneratePetData();
        }

        [Test(Description = "Successfully Find A Pet by Id")]
        [AllureTag("Positive")]
        public void FindPetById_Successfully()
        {
            // Create a new pet 
            wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.POST, pet);

            // Get the pet by its ID using a GET request
            var response = wrapper.ExecuteRequest($"{endpoints["Pet"]["Pet"]}{pet.id}", Method.GET);

            // Assert that the response content is equal to the serialized form of the pet object
            Assert.That(response.Content, Is.EqualTo(JsonSerializer.Serialize(pet)));

            Assert.That((int)response.StatusCode, Is.EqualTo(200), $"Response status code should be 200. Actual code: {(int)response.StatusCode}");
        }

        [Test(Description = "Find A Pet with Invalid pet id")]
        [AllureTag("Negative")]
        public void FindPetById_InvalidPetId()
        {
            // Send a GET request with an invalid pet ID
            var response = wrapper.ExecuteRequest($"{endpoints["Pet"]["Pet"]}InvalidPetId", Method.GET);

            Assert.That((int)response.StatusCode, Is.EqualTo(404), $"Response status code should be 404. Actual code: {(int)response.StatusCode}");
        }

        [Test(Description = "Find A Pet with wrong method type")]
        [AllureTag("Negative")]
        public void FindPetById_WrongMethodType()
        {
            // Send a PUT request instead of a GET request to get a pet by its ID
            var response = wrapper.ExecuteRequest($"{endpoints["Pet"]["Pet"]}{pet.id}", Method.PUT);

            JObject jsonResponse = JObject.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(405), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 405");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That((int)response.StatusCode, Is.EqualTo(405), $"Response status code should be 200. Actual code: {(int)response.StatusCode}");
            });
        }
    }
}
