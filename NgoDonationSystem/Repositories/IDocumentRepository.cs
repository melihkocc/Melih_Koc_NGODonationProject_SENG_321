using NgoDonationSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NgoDonationSystem.Repositories
{
    public interface IDocumentRepository : IRepository<Document>
    {
        Task<List<Document>> GetAllDocumentsOrderedAsync();
    }
}
