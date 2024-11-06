using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TicketingSystem.Dtos;
using TicketingSystem.Models;
using TicketingSystem.Services;
using TicketingSystem.Services.Interface;

namespace TicketingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly IIncidentService _incidentService;
        private readonly IWorkOrderService _workOrderService;
        private readonly INotificationService _notificationService;

        public TicketsController(IIncidentService incidentService, IWorkOrderService workOrderService, INotificationService notificationService)
        {
            _incidentService = incidentService;
            _workOrderService = workOrderService;
            _notificationService = notificationService;
        }

        [HttpPost("create-incident")]
        [Authorize]
        public async Task<IActionResult> CreateIncident([FromBody] IncidentDto incidentDto)
        {
            if (incidentDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var incident = await _incidentService.CreateIncident(incidentDto);
            return Ok(new { incident, status = "OPEN" });
        }

        [HttpPost("create-workorder")]
        [Authorize]
        public async Task<IActionResult> CreateWorkOrder([FromBody] WorkOrderDto workOrderDto)
        {
            if (workOrderDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var workOrder = await _workOrderService.CreateWorkOrder(workOrderDto);
            return Ok(new { workOrder, status = "ON PROGRESS" });
        }

        [HttpPost("send-notification")]
        [Authorize]
        public async Task<IActionResult> SendNotification([FromBody] NotificationDto notificationDto)
        {
            if (notificationDto == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _notificationService.SendNotification(notificationDto);
            return Ok();
        }

        [HttpPost("accept-workorder/{workOrderId}")]
        [Authorize]
        public async Task<IActionResult> AcceptWorkOrder(int workOrderId)
        {
            var workOrder = await _workOrderService.AcceptWorkOrder(workOrderId);
            return Ok(new { workOrder, status = "ON PROGRESS" });
        }

        [HttpPost("done-workorder/{workOrderId}")]
        [Authorize]
        public async Task<IActionResult> DoneWorkOrder(int workOrderId)
        {
            var workOrder = await _workOrderService.MarkWorkOrderAsDone(workOrderId);
            return Ok(new { workOrder, status = "CLOSE" });
        }

        [HttpPost("done-incident/{incidentId}")]
        [Authorize]
        public async Task<IActionResult> DoneIncident(int incidentId)
        {
            var incident = await _incidentService.MarkIncidentAsDone(incidentId);
            return Ok(new { incident, status = "CLOSE" });
        }

        [HttpGet("list-incidents")]
        [Authorize]
        public async Task<IActionResult> ListIncidents()
        {
            var incidents = await _incidentService.ListIncidents();
            return Ok(incidents);
        }

        [HttpGet("list-workorders")]
        [Authorize]
        public async Task<IActionResult> ListWorkOrders()
        {
            var workOrders = await _workOrderService.ListWorkOrders();
            return Ok(workOrders);
        }
    }
}
