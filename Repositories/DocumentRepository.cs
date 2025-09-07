using ASCO.Models;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using ASCO.DbContext;
using ASCO.DTOs;


namespace ASCO.Repositories
{
    public class DocumentRepository
    {
        private readonly ASCODbContext _context;

        public DocumentRepository(ASCODbContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Document doc)
        {
            await _context.Documents.AddAsync(doc);
            return await _context.SaveChangesAsync();
        }

        public async Task<Document?> GetByIdAsync(Guid id)
        {
            return await _context.Documents.FindAsync(id);
        }

        public async Task<List<Document>> GetAllAsync()
        {
            return await _context.Documents.ToListAsync();
        }
    }
}