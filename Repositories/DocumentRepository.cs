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

        public async Task<int> AddDocumentTextAsync(DocumentText doc)
        {

            await _context.DocumentTexts.AddAsync(doc);
            return await _context.SaveChangesAsync();
        }

        public async Task<List<(Document Document, DocumentText DocumentText)>> GetDocumentWithTextAsync(string? search)
        {
            var query =
            from d in _context.Documents
            join dt in _context.DocumentTexts
                on d.Id equals dt.DocumentId into dtGroup
            from dt in dtGroup.DefaultIfEmpty() // LEFT JOIN
            select new { d, dt };

            if (!string.IsNullOrWhiteSpace(search))
            {
                var trimmed = search.Trim();
                var pattern = $"%{trimmed}%"; // Postgres ILIKE

                query = query.Where(x =>
                    EF.Functions.ILike(x.d.Name, pattern) ||
                    (x.dt != null && EF.Functions.ILike(x.dt.Content, pattern))
                );
            }

            var results = await query.ToListAsync();

            return results.Select(x => (x.d, x.dt)).ToList();
        }
        public async Task<int> AddApprovalsAsync(List<DocumentApproval> approvals)
        {
            await _context.DocumentApprovals.AddRangeAsync(approvals);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> GetDocumentVersionNumberAsync(Guid id)
        {
            return await _context.DocumentVersions.CountAsync(d => d.DocumentId == id);
        }

        public async Task<int> AddVersionAsync(DocumentVersion add)
        {
            await _context.DocumentVersions.AddAsync(add);
            return await _context.SaveChangesAsync();
        }

        public async Task<DocumentVersion?> GetVersionByIdAsync(Guid id)
        {
            return await _context.DocumentVersions.FindAsync(id);
        }
        public async Task<List<DocumentVersion>> FetchAllVersionsAsync(Guid id)
        {
            return await _context.DocumentVersions
            .Where(d => d.DocumentId == id)
            .OrderBy(d => d.VersionNumber)
            .ToListAsync();
        }
        public async Task<Document?> GetByIdAsync(Guid id)
        {
            return await _context.Documents.FindAsync(id);
        }

        public async Task<List<Document>> GetAllAsync()
        {
            return await _context.Documents.ToListAsync();
        }

        public async Task<(Form,List<FormField>)> CreateFormWithFieldsAsync(Form form, List<FormField> fields)
        {
            await _context.Forms.AddAsync(form);
            await _context.SaveChangesAsync(); // Save to get the generated Form ID

            foreach (var field in fields)
            {
                field.FormId = form.Id; // Associate field with the form
            }

            await _context.FormFields.AddRangeAsync(fields);
            await _context.SaveChangesAsync();
            return (form,fields);
        }

        // public async Task<List<Form>> GetAllFormsWithFieldsAsync()
        // {
        //     return await _context.Forms.Include(f => f.FormFields).ToListAsync();
        // }
        public async Task<List<Form>> GetAllFormsAsync()
        {
            return await _context.Forms.ToListAsync();
        }

        public async Task<Form?> GetFormByIdAsync(int id)
        {
            return await _context.Forms.FindAsync(id);
        }

        public async Task<List<FormField>> GetFieldsByFormIdAsync(int formId)
        {
            return await _context.FormFields.Where(f => f.FormId == formId).ToListAsync();
        }
    }
}