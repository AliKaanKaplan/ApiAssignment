using Bogus;

namespace MeDirectApiProject.Helpers
{
    public class UserGenerator
    {
        // Creates a Faker instance for generating fake data.
        private static readonly Faker faker = new Faker();

        // Generates random user data using the Faker library.

        /// <summary>
        /// Generates fake order data.
        /// </summary>
        public static User GenerateUserData()
        {
            // Generates a random user ID within the range of 1000 to 9999.
            var userId = faker.Random.Int(1000, 9999);

            // Generates a random username
            string username = faker.Internet.UserName();

            // Generates a random first name 
            var firstName = faker.Name.FirstName();

            // Generates a random last name 
            var lastName = faker.Name.LastName();

            // Generates a random email address
            var email = faker.Internet.Email();

            // Generates a random password 
            string password = faker.Internet.Password();

            // Generates a random phone number 
            var phone = faker.Phone.PhoneNumber();

            // Generates a random user status (0 or 1) using the Random.Int() method.
            var userStatus = faker.Random.Int(0, 1);

            // Creates a new User object with the generated data.
            return new User
            {
                id = userId,
                username = username,
                firstName = firstName,
                lastName = lastName,
                email = email,
                password = password,
                phone = phone,
                userStatus = userStatus
            };
        }
    }
}
