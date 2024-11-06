using System.Collections.Generic;
using System.Threading.Tasks;
using TicketingSystem.Dtos;
using TicketingSystem.Models;

namespace TicketingSystem.Services.Interface
{
    public interface IWorkOrderService 
    {
        Task<WorkOrder> CreateWorkOrder(WorkOrderDto workOrderDto); 
        Task<WorkOrder> AcceptWorkOrder(int workOrderId); 
        Task<WorkOrder> MarkWorkOrderAsDone(int workOrderId); 
        Task<IEnumerable<WorkOrder>> ListWorkOrders(); 
    }
}
