using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.DTOs.Responses;
using NgoDonationSystem.Models;
using NgoDonationSystem.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repo;

        public AuditLogService(IAuditLogRepository repo) { _repo = repo; }

        public async Task<List<AuditLogResponse>> GetAllAsync()
        {
            var logs = await _repo.GetAllLogsWithUserAsync();

            return logs.Select(al => new AuditLogResponse
            {
                Id = al.Id, TableName = al.TableName, Action = al.Action,
                UserName = al.User != null ? $"{al.User.FirstName} {al.User.LastName}" : "Sistem",
                Timestamp = al.Timestamp, OldValues = al.OldValues, NewValues = al.NewValues
            }).ToList();
        }

        public async Task<AuditLogResponse?> GetByIdAsync(int id)
        {
            var al = await _repo.GetLogWithUserByIdAsync(id);
            if (al == null) return null;
            return new AuditLogResponse
            {
                Id = al.Id, TableName = al.TableName, Action = al.Action,
                UserName = al.User != null ? $"{al.User.FirstName} {al.User.LastName}" : "Sistem",
                Timestamp = al.Timestamp, OldValues = al.OldValues, NewValues = al.NewValues
            };
        }
    }
}
