using System.Threading.Tasks;
using TicketingSystem.Dtos;

namespace TicketingSystem.Services.Interface
{
    public interface INotificationService
    {
        Task SendNotification(NotificationDto notificationDto); 
    }
}
