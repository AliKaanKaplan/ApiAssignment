using MeDirectApiProject.Helpers;
using MeDirectApiProject.TestScenarios;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Allure.Attributes;
using NUnit.Allure.Core;
using RestSharp;

namespace MeDirectApiProject.StoreFeatures
{
    // NUnit Allure attributes for reporting
    [AllureNUnit]
    [AllureEpic("Store")]
    [AllureFeature("Return Pet Inventories Feature")]
    [AllureSuite("Store Suite")]

    public class ReturnPetInventories : BaseTest
    {
        private Pet pet;
        private PetInventories oldPetInv, newPetInv;

        [SetUp]
        public void Setup()
        {
            pet = PetGenerator.GeneratePetData();
        }

        [Test(Description = "Successfully Returning Pet Environment")]
        [AllureTag("Positive")]
        public void ReturnEnvironment_Successfully()
        {
            var oldResponse = wrapper.ExecuteRequest(endpoints["Store"]["Inventory"], Method.GET);
            oldPetInv = JsonConvert.DeserializeObject<PetInventories>(oldResponse.Content);

            wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.POST, pet);

            var newResponse = wrapper.ExecuteRequest(endpoints["Store"]["Inventory"], Method.GET);
            newPetInv = JsonConvert.DeserializeObject<PetInventories>(newResponse.Content);

            switch (pet.status)
            {
                case "Available":
                    Assert.IsTrue(oldPetInv.available < newPetInv.available, "The pet added with the 'Available' status did not affect the pet inventory.");
                    break;
                case "Pending":
                    Assert.IsTrue(oldPetInv.pending < newPetInv.pending, "The pet added with the 'Pending' status did not affect the pet inventory");
                    break;
                case "Sold":
                    Assert.IsTrue(oldPetInv.sold < newPetInv.sold, "The pet added with the 'Sold' status did not affect the pet inventory");
                    break;
                default:
                    Assert.Fail($"Invalid pet status: {pet.status}");
                    break;
            }
        }

        [Test(Description = "Returning Pet Environment with wrong method type")]
        [AllureTag("Negative")]
        public void ReturnEnvironment_WrongMethodType()
        {
            var response = wrapper.ExecuteRequest(endpoints["Store"]["Inventory"], Method.DELETE);

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
