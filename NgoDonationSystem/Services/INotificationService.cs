using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface INotificationService
    {
        Task<List<Notification>> GetUnreadNotificationsAsync(int userId);
        Task CreateNotificationAsync(int userId, string title, string message, string? actionUrl = null);
        Task MarkAsReadAsync(int notificationId);
        Task MarkAllAsReadAsync(int userId);
    }
}
