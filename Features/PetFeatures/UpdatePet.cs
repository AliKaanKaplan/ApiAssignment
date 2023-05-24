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
    [AllureFeature("Update pet Feature")]
    [AllureSuite("Pet Suite")]
    public class UpdatePet : BaseTest
    {
        private Pet pet, newPet;

        [SetUp]
        public void Setup()
        {
            pet = PetGenerator.GeneratePetData();
            wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.POST, pet);

            newPet = PetGenerator.GeneratePetData();
        }

        [Test(Description = "Successfully Update pet")]
        [AllureTag("Positive")]
        public void UpdatePet_Successfully()
        {
            // Set the ID of the new pet as the ID of the original pet
            newPet.id = pet.id;

            // Update the pet
            var response = wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.PUT, newPet);

            Assert.That(response.Content, Is.EqualTo(JsonSerializer.Serialize(newPet)));
            Assert.That((int)response.StatusCode, Is.EqualTo(200), $"Response status code should be 200. Actual code: {(int)response.StatusCode}");
        }

        [Test(Description = "Fails to update a pet with wrong method type")]
        [AllureTag("Negative")]
        public void UpdatePet_WrongMethodType()
        {
            // Set the ID of the new pet as the ID of the original pet
            newPet.id = pet.id;

            // Attempt to update the pet using the wrong method type (DELETE)
            var response = wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.DELETE, newPet);
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
