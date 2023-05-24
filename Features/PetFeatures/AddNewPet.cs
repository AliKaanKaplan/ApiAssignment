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
    [AllureFeature("Add pet Feature")]
    [AllureSuite("Pet Suite")]

    public class AddNewPet : BaseTest
    {
        private Pet pet;

        [SetUp]
        public void Setup()
        {
            pet = PetGenerator.GeneratePetData();
        }

        [Test(Description = "Successfully Adding A Pet")]
        [AllureTag("Positive")]
        public void AddAPet_Successfully()
        {
            var response = wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.POST, pet);

            Assert.That(response.Content, Is.EqualTo(JsonSerializer.Serialize(pet)));
            Assert.That((int)response.StatusCode, Is.EqualTo(200), "Response status code should be 200");
        }

        [Test(Description = "Adding A Pet with empty body")]
        [AllureTag("Negative")]
        public void AddAPet_EmptyBody()
        {
            var response = wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.POST, " ");

            Assert.That((int)response.StatusCode, Is.EqualTo(500), "Response status code should be 500");
        }

        [Test(Description = "Adding A Pet with wrong method type")]
        [AllureTag("Negative")]
        public void AddAPet_WrongMethodType()
        {
            var response = wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.DELETE, pet);

            JObject jsonResponse = JObject.Parse(response.Content);

            Assert.Multiple(() =>
            {
                Assert.That(jsonResponse["code"].Type, Is.EqualTo(JTokenType.Integer), "Code should be of type integer");
                Assert.That((int)jsonResponse["code"], Is.EqualTo(405), $"Actual code: {(int)jsonResponse["code"]}, Expected code: 405");
                Assert.That(jsonResponse["type"].Type, Is.EqualTo(JTokenType.String), "Type should be of type string");
                Assert.That((string)jsonResponse["type"], Is.EqualTo("unknown"), $"Actual type: {(string)jsonResponse["type"]}, Expected type: unknown");
                Assert.That((int)response.StatusCode, Is.EqualTo(405), $"Response status code should be 405. Actual code: {(int)response.StatusCode}");
            });
        }
    }
}
