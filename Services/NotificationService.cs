using System.Threading.Tasks;
using TicketingSystem.Dtos;
using TicketingSystem.Services.Interface;

namespace TicketingSystem.Services
{
    public class NotificationService : INotificationService
    {
        public async Task SendNotification(NotificationDto notificationDto)
        {
            //WhatsApp message
            await Task.CompletedTask;
        }
    }
}
