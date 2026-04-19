using Microsoft.EntityFrameworkCore;
using NgoDonationSystem.Models;
using NgoDonationSystem.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NgoDonationSystem.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _repo;

        public DocumentService(IDocumentRepository repo) { _repo = repo; }

        public async Task<List<Document>> GetAllAsync()
        {
            return await _repo.GetAllDocumentsOrderedAsync();
        }

        public async Task<Document?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task AddAsync(Document document)
        {
            await _repo.AddAsync(document);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
