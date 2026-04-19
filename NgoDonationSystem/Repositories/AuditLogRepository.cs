using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Data;
using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<AuditLog>> GetAllLogsWithUserAsync()
        {
            return await _dbSet
                .Include(al => al.User)
                .OrderByDescending(al => al.Timestamp)
                .ToListAsync();
        }

        public async Task<AuditLog?> GetLogWithUserByIdAsync(int id)
        {
            return await _dbSet.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
