using System;

namespace TicketingSystem.Models
{
    public class Incident
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // OPEN, ON PROGRESS, CLOSE
        public DateTime CreatedAt { get; set; }
    }
}
