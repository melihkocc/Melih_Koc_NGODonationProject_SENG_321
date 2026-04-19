using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public interface IDocumentService
    {
        Task<List<Document>> GetAllAsync();
        Task<Document?> GetByIdAsync(int id);
        Task AddAsync(Document document);
        Task DeleteAsync(int id);
    }
}
