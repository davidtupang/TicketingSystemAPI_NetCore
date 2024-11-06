using Microsoft.EntityFrameworkCore;
using TicketingSystem.Data;
using TicketingSystem.Models;
using TicketingSystem.Repositories.Interface;

namespace TicketingSystem.Repositories
{
    public class WorkOrderRepository : IWorkOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(WorkOrder workOrder)
        {
            await _context.WorkOrders.AddAsync(workOrder);
            await _context.SaveChangesAsync();
        }

        public async Task<WorkOrder> GetByIdAsync(int id)
        {
            return await _context.WorkOrders.Include(w => w.Incident).FirstOrDefaultAsync(w => w.Id == id);
        }

        public async Task UpdateAsync(WorkOrder workOrder)
        {
            _context.WorkOrders.Update(workOrder);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<WorkOrder>> GetAllAsync()
        {
            return await _context.WorkOrders.Include(w => w.Incident).ToListAsync();
        }
    }

}
