using Microsoft.EntityFrameworkCore;
using TicketingSystem.Data;
using TicketingSystem.Models;
using TicketingSystem.Repositories.Interface;

namespace TicketingSystem.Repositories
{
    public class IncidentRepository : IIncidentRepository
    {
        private readonly ApplicationDbContext _context;

        public IncidentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Incident incident)
        {
            await _context.Incidents.AddAsync(incident);
            await _context.SaveChangesAsync();
        }

        public async Task<Incident> GetByIdAsync(int id)
        {
            return await _context.Incidents.FindAsync(id);
        }

        public async Task UpdateAsync(Incident incident)
        {
            _context.Incidents.Update(incident);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Incident>> GetAllAsync()
        {
            return await _context.Incidents.ToListAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }

}
