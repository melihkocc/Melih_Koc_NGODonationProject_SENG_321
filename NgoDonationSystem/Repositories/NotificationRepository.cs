using NgoDonationSystem.Data;
using NgoDonationSystem.Models;

namespace NgoDonationSystem.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
