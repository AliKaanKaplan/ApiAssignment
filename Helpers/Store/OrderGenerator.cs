using Bogus;

namespace MeDirectApiProject.Helpers
{
    public static class OrderGenerator
    {
        // Faker instance used to generate fake data.
        private static readonly Faker faker = new Faker();

        /// <summary>
        /// Generates fake order data.
        /// </summary>
        public static Order GenerateOrderData()
        {
            // Generate a random order ID.
            int orderId = faker.Random.Int(1, 10);

            // Generate a random pet ID.
            var petId = faker.Random.Int(1000, 9999);

            // Generate a random quantity.
            var quantity = faker.Random.Int(1, 10);

            // Set a fixed ship date.
            var shipDate = "2023-05-19T20:07:42.933+0000";

            // Set the status of the order.
            var status = "placed";

            // Generate a random boolean value to represent order completion.
            var complete = faker.Random.Bool();

            // Create and return the order object with generated data.
            return new Order
            {
                id = orderId,
                petId = petId,
                quantity = quantity,
                shipDate = shipDate,
                status = status,
                complete = complete
            };
        }
    }
}
