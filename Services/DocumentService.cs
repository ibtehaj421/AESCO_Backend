using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ASCO.Models;
using ASCO.Repositories;
using ASCO.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ASCO.Services
{
    public class DocumentService
    {
        private readonly DocumentRepository _documentRepository;
        private readonly string _basePath;

        public DocumentService(DocumentRepository documentRepository, IWebHostEnvironment env)
        {
            _documentRepository = documentRepository;
            _basePath = Path.Combine(env.ContentRootPath, "uploads");
        }

        //create method
        public async Task<DocDto> CreateFolderAsync(CreateFolderDto dto)
        {
            // Validate parent
            Document? parent = null;
            if (dto.ParentId.HasValue)
            {
                parent = await _documentRepository.GetByIdAsync(dto.ParentId.Value);
                if (parent == null || !parent.IsFolder)
                    throw new InvalidOperationException("Invalid parent folder");
            }

            // Compute relative path
            var relativePath = parent != null
                ? Path.Combine(parent.PhysicalPath, dto.Name)
                : dto.Name;

            // Ensure directory exists on disk
            var folderPath = Path.Combine(_basePath, relativePath);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Create metadata record
            var folder = new Document
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                PhysicalPath = relativePath.Replace("\\", "/"),
                IsFolder = true,
                ParentId = dto.ParentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedById = dto.createdby,
                UpdatedById = dto.updatedby
            };

            var value = await _documentRepository.AddAsync(folder);
            if (value < 1)
            {
                //no changes were made and folder could not be created.
                return new DocDto();
            }

            return folder.ToDto();
        }

        //upload method
        public async Task<DocDto> UploadFileAsync(UploadFileDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                throw new InvalidOperationException("File is empty");

            Document? parent = null;
            if (dto.ParentId.HasValue)
            {
                parent = await _documentRepository.GetByIdAsync(dto.ParentId.Value);
                if (parent == null || !parent.IsFolder)
                    throw new InvalidOperationException("Invalid parent folder");
            }

            var relativePath = parent != null
                ? Path.Combine(parent.PhysicalPath, dto.File.FileName)
                : dto.File.FileName;

            var filePath = Path.Combine(_basePath, relativePath);
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var file = new Document
            {
                Id = Guid.NewGuid(),
                Name = dto.File.FileName,
                PhysicalPath = relativePath.Replace("\\", "/"),
                IsFolder = false,
                ParentId = dto.ParentId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CreatedById = dto.CreatedBy,
                UpdatedById = dto.UpdatedBy
            };

            await _documentRepository.AddAsync(file);

            return file.ToDto();
        }

        public async Task<List<DocDto>> GetAllByHierarchyAsync()
        {
            //get all documents.
            var docs = await _documentRepository.GetAllAsync();
            var dtos = docs.Select(DocumentMapper.ToDto).ToList();

            var lookup = dtos.ToDictionary(d => d.Id, d => d);
            List<DocDto> roots = new List<DocDto>();

            foreach (var doc in dtos)
            {
                if (doc.ParentId.HasValue && lookup.ContainsKey(doc.ParentId.Value))
                {
                    lookup[doc.ParentId.Value].Children.Add(doc);
                }
                else
                {
                    roots.Add(doc); // top-level (no parent)
                }
            }

            return roots;

        }

        public async Task<(Document? Meta, string? FilePath)> GetDocumentWithFileAsync(Guid id)
        {
            //get the document by id.
            var doc = await _documentRepository.GetByIdAsync(id);
            Console.WriteLine(doc.PhysicalPath);
            if (doc == null || doc.IsFolder) return (null, null);
            string filepath = "uploads/" + doc.PhysicalPath;
            return (doc, filepath);
        }
    }
    

    public class DocumentStorageOptions
    {
        public string BasePath { get; set; } = string.Empty;
    }

}