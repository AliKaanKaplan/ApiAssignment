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
    [AllureFeature("Upload pet image Feature")]
    [AllureSuite("Pet Suite")]
    public class UploadAnImage : BaseTest
    {
        private Pet pet;
        private string imageFilePath, txtFilePath;
        private byte[] imageBytes;

        [SetUp]
        public void Setup()
        {
            // Define file paths and read image bytes from the file
            imageFilePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "..\\Utils\\RestSharpLogo.png");
            txtFilePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.FullName, "..\\Utils\\InvalidFile.txt");
            imageBytes = File.ReadAllBytes(imageFilePath);

            // Assert that the image and text files exist
            Assert.IsTrue(File.Exists(imageFilePath), $"File not found: {imageFilePath}");
            Assert.IsTrue(File.Exists(txtFilePath), $"File not found: {txtFilePath}");

            pet = PetGenerator.GeneratePetData();
        }

        [Test(Description = "Successfully uploads a pet image")]
        [AllureTag("Positive")]
        public void UploadPetImage_Successfully()
        {
            // Create the pet
            wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.POST, pet);

            // Upload the pet image
            var response = wrapper.UploadImage(pet.id, imageFilePath, Method.POST);
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

        [Test(Description = "Fails to upload a pet image with a wrong file")]
        [AllureTag("Negative")]
        public void UploadPetImage_WrongFile()
        {
            // Create the pet
            wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.POST, pet);

            // Upload an image with a wrong file type
            var response = wrapper.UploadImage(pet.id, txtFilePath, Method.POST);

            Assert.That((int)response.StatusCode, Is.EqualTo(415), $"Response status code should be 415 (Unsupported Media Type). Actual code: {(int)response.StatusCode}");
        }

        [Test(Description = "Fails to upload a pet image with a wrong method type")]
        [AllureTag("Negative")]
        public void UploadPetImage_WrongMethod()
        {
            // Create the pet
            wrapper.ExecuteRequest(endpoints["Pet"]["Pet"], Method.POST, pet);

            // Upload an image with a wrong method type
            var response = wrapper.UploadImage(pet.id, imageFilePath, Method.GET);
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
