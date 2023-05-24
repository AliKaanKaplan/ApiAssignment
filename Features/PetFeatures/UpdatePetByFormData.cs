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
    [AllureFeature("Update pet Feature")]
    [AllureSuite("Pet Suite")]
    public class UpdatePetByFormData : BaseTest
    {
        private Pet pet, newPet;
        private dynamic body;

        [SetUp]
        public void Setup()
        {
            // Generate original and new pet data
            pet = PetGenerator.GeneratePetData();
            newPet = PetGenerator.GeneratePetData();

            // Create the original pet
            wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.POST, pet);

            body = new
            {
                name = newPet.name,
                status = newPet.status
            };
        }

        [Test(Description = "Successfully Update pet by form data")]
        [AllureTag("Positive")]
        public void UpdatePetByFormData_Successfully()
        {
            // Update the pet using form data
           var response = wrapper.ExecuteRequest($"{endpoints["Pet"]["Pet"]}{pet.id}", Method.POST, body, contentType: "multipart/form-data");
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

        [Test(Description = "Update pet by form data with No-exist pet")]
        [AllureTag("Negative")]
        public void UpdatePetByFormData_NoExistPet()
        {
            var headers = wrapper.GetHeaders("special-key");

            // Delete the original pet
            wrapper.ExecuteRequest($"{endpoints["Pet"]["Pet"]}{pet.id}", Method.DELETE, headers);

            // Attempt to update the non-existing pet using form data
            var response = wrapper.ExecuteRequest($"{endpoints["Pet"]["Pet"]}{pet.id}", Method.POST, body, contentType: "multipart/form-data");
            JObject jsonResponse = JObject.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(404), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 404");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That(jsonResponse["message"].Type, Is.EqualTo(JTokenType.String), "Message should be of type string");
                Assert.That((string)jsonResponse["message"], Is.EqualTo("not found"), $"Actual message: {(string)jsonResponse["message"]}, Expected message: not found");
                Assert.That((int)response.StatusCode, Is.EqualTo(404), $"Response status code should be 404. Actual code: {(int)response.StatusCode}");
            });
        }
    }
}
