using NgoDonationSystem.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IAuditLogService
    {
        Task<List<AuditLogResponse>> GetAllAsync();
        Task<AuditLogResponse?> GetByIdAsync(int id);
    }
}
