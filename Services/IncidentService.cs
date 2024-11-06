using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystem.Dtos;
using TicketingSystem.Models;
using TicketingSystem.Repositories.Interface;
using TicketingSystem.Services.Interface;

namespace TicketingSystem.Services
{
    public class IncidentService : IIncidentService
    {
        private readonly IIncidentRepository _incidentRepository;

        public IncidentService(IIncidentRepository incidentRepository)
        {
            _incidentRepository = incidentRepository;
        }

        // Create a new incident
        public async Task<Incident> CreateIncident(IncidentDto incidentDto)
        {
            var incident = new Incident
            {
                ClientName = incidentDto.ClientName,
                Description = incidentDto.Description,
                Status = "OPEN", 
                CreatedAt = DateTime.UtcNow
            };

            await _incidentRepository.CreateAsync(incident);
            return incident; // Return the created incident
        }

        
        public async Task<Incident> MarkIncidentAsDone(int incidentId)
        {
            var incident = await _incidentRepository.GetByIdAsync(incidentId);
            if (incident != null)
            {
                incident.Status = "CLOSE"; // Update status to CLOSE
                await _incidentRepository.UpdateAsync(incident);
            }

            return incident;
        }

        // List all incidents
        public async Task<IEnumerable<Incident>> ListIncidents()
        {
            return await _incidentRepository.GetAllAsync(); 
        }
    }
}
