using Bogus;

namespace MeDirectApiProject.Helpers
{
    public static class PetGenerator
    {
        // Faker instance used to generate fake data.
        private static readonly Faker faker = new Faker();

        /// <summary>
        /// Generates fake pet data.
        /// </summary>
        public static Pet GeneratePetData()
        {
            // Generate a random pet ID.
            var petId = faker.Random.Int(1000, 9999);

            // Generate a random category ID.
            var categoryId = faker.Random.Int(1000, 9999);

            // Generate a random category name using commerce categories.
            var categoryName = faker.Commerce.Categories(1)[0];

            // Generate a random pet name.
            var petName = faker.Name.FirstName();

            // Generate a random photo URL.
            var photoUrl = faker.Internet.Url();

            // Generate a random tag ID.
            var tagId = faker.Random.Int(1000, 9999);

            // Generate a random tag name using commerce products.
            var tagName = faker.Commerce.Product();

            // Generate a random status from a predefined list.
            var status = faker.PickRandom(new[] { "Available", "Pending", "Sold" });

            // Create and return the pet object with generated data.
            return new Pet
            {
                id = petId,
                category = new Category { id = categoryId, name = categoryName },
                name = petName,
                photoUrls = new[] { photoUrl },
                tags = new[] { new Tag { id = tagId, name = tagName } },
                status = status
            };
        }
    }
}
