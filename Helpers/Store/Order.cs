namespace MeDirectApiProject.Helpers
{
    public class Order
    {
        public int id { get; set; }
        public int petId { get; set; }
        public int quantity { get; set; }
        public string shipDate { get; set; }
        public string status { get; set; }
        public bool complete { get; set; }
    }

}
