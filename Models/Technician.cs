namespace TicketingSystem.Models
{
    public class Technician
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // internal, external
        public string PhoneNumber { get; set; }
    }
}
