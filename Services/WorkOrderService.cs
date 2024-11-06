using TicketingSystem.Dtos;
using TicketingSystem.Models;
using TicketingSystem.Repositories.Interface;
using TicketingSystem.Services.Interface;

namespace TicketingSystem.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IWorkOrderRepository _workOrderRepository;

        public WorkOrderService(IWorkOrderRepository workOrderRepository)
        {
            _workOrderRepository = workOrderRepository;
        }

        public async Task<WorkOrder> CreateWorkOrder(WorkOrderDto workOrderDto)
        {
            var workOrder = new WorkOrder
            {
                IncidentId = workOrderDto.IncidentId,
                Status = "OPEN",
                CreatedAt = DateTime.UtcNow
            };

            await _workOrderRepository.CreateAsync(workOrder);
            return workOrder;
        }

        public async Task<WorkOrder> AcceptWorkOrder(int workOrderId)
        {
            var workOrder = await _workOrderRepository.GetByIdAsync(workOrderId);
            if (workOrder != null)
            {
                workOrder.Status = "ON PROGRESS";
                await _workOrderRepository.UpdateAsync(workOrder);
            }

            return workOrder;
        }

        public async Task<WorkOrder> MarkWorkOrderAsDone(int workOrderId)
        {
            var workOrder = await _workOrderRepository.GetByIdAsync(workOrderId);
            if (workOrder != null)
            {
                workOrder.Status = "CLOSE";
                await _workOrderRepository.UpdateAsync(workOrder);
            }

            return workOrder;
        }

        public async Task<IEnumerable<WorkOrder>> ListWorkOrders()
        {
            return await _workOrderRepository.GetAllAsync();
        }
    }

}
