using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IAuditLogRepository : IRepository<AuditLog>
    {
        Task<List<AuditLog>> GetAllLogsWithUserAsync();
        Task<AuditLog?> GetLogWithUserByIdAsync(int id);
    }
}
