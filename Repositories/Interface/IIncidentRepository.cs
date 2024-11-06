using System.Threading.Tasks;
using TicketingSystem.Models;

namespace TicketingSystem.Repositories.Interface
{
    public interface IIncidentRepository
    {
        Task<Incident> GetByIdAsync(int id);
        Task CreateAsync(Incident incident); // Declaration only
        Task UpdateAsync(Incident incident);
        Task DeleteAsync(int id);
        Task<IEnumerable<Incident>> GetAllAsync();
    }
}
