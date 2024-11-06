using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystem.Models;

namespace TicketingSystem.Repositories.Interface
{
    public interface IWorkOrderRepository 
    {
        Task CreateAsync(WorkOrder workOrder);
        Task<WorkOrder> GetByIdAsync(int id); 
        Task UpdateAsync(WorkOrder workOrder); 
        Task<IEnumerable<WorkOrder>> GetAllAsync(); 
    }
}
