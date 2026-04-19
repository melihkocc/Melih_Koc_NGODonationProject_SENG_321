using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using NgoDonationSystem.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly ApplicationDbContext _context;

        public NotificationService(INotificationRepository repo, ApplicationDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<List<Notification>> GetUnreadNotificationsAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task CreateNotificationAsync(int userId, string title, string message, string? actionUrl = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                ActionUrl = actionUrl,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _repo.AddAsync(notification);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _repo.GetByIdAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _repo.UpdateAsync(notification);
            }
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            if (notifications.Any())
            {
                await _context.SaveChangesAsync();
            }
        }
    }
}
