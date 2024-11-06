namespace TicketingSystem.Models
{
    public class WorkOrder
    {
        public int Id { get; set; }
        public int IncidentId { get; set; } // Foreign key to Incident
        public string Status { get; set; } // OPEN, CLOSE
        public DateTime CreatedAt { get; set; }

        // Navigation property
        public Incident Incident { get; set; }
    }
}
