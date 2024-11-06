using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystem.Dtos;
using TicketingSystem.Models;

namespace TicketingSystem.Services.Interface
{
    public interface IIncidentService
    {
        Task<Incident> CreateIncident(IncidentDto incidentDto); 
        Task<Incident> MarkIncidentAsDone(int incidentId); 
        Task<IEnumerable<Incident>> ListIncidents(); 
    }
}
