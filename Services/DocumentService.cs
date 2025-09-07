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
            if (doc == null || doc.IsFolder) return (null, null);
            string filepath = "uploads/" + doc.PhysicalPath;
            return (doc, filepath);
        }

        public async Task<FormTemplateDTO> CreateFormAsync(FormTemplateDTO dto)
        {
            if (dto.Form == null || dto.Fields == null || !dto.Fields.Any())
                throw new InvalidOperationException("Form and fields are required");

            var form = new Form
            {
                Title = dto.Form.Title,
                Description = dto.Form.Description
            };
            List<FormField> fields = new List<FormField>();


            foreach (var fieldDto in dto.Fields)
            {
                var field = new FormField
                {
                    //FormId = form.Id,
                    FieldName = fieldDto.FieldName,
                    FieldType = fieldDto.FieldType,
                    HelpText = fieldDto.HelpText,
                    ValidValue = fieldDto.ValidValue,
                    IsRequired = fieldDto.IsRequired
                };
                fields.Add(field);
            }
            // Save form and fields in a transaction
            var (form1,fields1) = await _documentRepository.CreateFormWithFieldsAsync(form, fields);
            if (form1.Id < 1)
            {
                throw new InvalidOperationException("Failed to create form");
            }
            // Map back to DTO
            var resultDto = new FormTemplateDTO
            {
                Form = new FormDTO
                {
                    Id = form1.Id,
                    Title = form1.Title,
                    Description = form1.Description
                },
                Fields = fields1.Select(f => new FormFieldDTO
                {
                    Id = f.Id,
                    FormId = form1.Id,
                    FieldName = f.FieldName,
                    FieldType = f.FieldType,
                    HelpText = f.HelpText,
                    ValidValue = f.ValidValue,
                    IsRequired = f.IsRequired
                }).ToList()
            };

            return resultDto;
        }

        public async Task<List<Form>> GetAllFormTemplatesAsync()
        {
            var forms = await _documentRepository.GetAllFormsAsync();
            
            return forms;
        }
        
        public async Task<FormTemplateDTO?> GetFormTemplateByIdAsync(int id)
        {
            var form = await _documentRepository.GetFormByIdAsync(id);
            if (form == null) return null;

            var fields = await _documentRepository.GetFieldsByFormIdAsync(form.Id);
            var formDto = new FormDTO
            {
                Id = form.Id,
                Title = form.Title,
                Description = form.Description
            };
            var fieldDtos = fields.Select(f => new FormFieldDTO
            {
                Id = f.Id,
                FormId = f.FormId,
                FieldName = f.FieldName,
                FieldType = f.FieldType,
                HelpText = f.HelpText,
                ValidValue = f.ValidValue,
                IsRequired = f.IsRequired
            }).ToList();

            return new FormTemplateDTO
            {
                Form = formDto,
                Fields = fieldDtos
            };
        }
    }
    

    public class DocumentStorageOptions
    {
        public string BasePath { get; set; } = string.Empty;
    }

}