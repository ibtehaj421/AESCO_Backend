using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ASCO.Models;
using ASCO.Repositories;
using ASCO.DTOs;
using Microsoft.AspNetCore.Mvc;
using UglyToad.PdfPig;
using DocumentFormat.OpenXml.Packaging;
namespace ASCO.Services
{
    public class DocumentService
    {
        private readonly DocumentRepository _documentRepository;
        private readonly string _basePath;
        private readonly string _updatePath;

        public DocumentService(DocumentRepository documentRepository, IWebHostEnvironment env)
        {
            _documentRepository = documentRepository;
            _basePath = Path.Combine(env.ContentRootPath, "uploads");
            _updatePath = Path.Combine(env.ContentRootPath, "uploads/updates");
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

            var value = await _documentRepository.AddAsync(file);
            if (value < 1)
            {
                return new DocDto(); //null entity and not to save the file on the system.
            }
            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            //now save the file text in the document text table and also fill the approval table.
            string text = "";
            try
            {
                text = ExtractText(filePath);
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //once text is extracted 
            var textStore = new DocumentText
            {
                DocumentId = file.Id,
                Content = text
            };
            await _documentRepository.AddDocumentTextAsync(textStore);
            //now save the approval request users.
            List<DocumentApproval> approvals = new List<DocumentApproval>();

            foreach (int u in dto.Users)
            {
                var approval = new DocumentApproval
                {
                    Id = Guid.NewGuid(),
                    DocumentId = file.Id,
                    ApproverId = u,

                };
                approvals.Add(approval);
            }
            await _documentRepository.AddApprovalsAsync(approvals);
            return file.ToDto();
        }
        public async Task<string> UpdateDocumentAsync(UpdateDocDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                throw new InvalidOperationException("File is empty");

            Document? parent = null;
            int version;
            if (dto.ParentId != Guid.Empty)
            {
                parent = await _documentRepository.GetByIdAsync(dto.ParentId);
                if (parent == null)
                    throw new InvalidOperationException("Invalid parent");
            }
            var relativePath = Path.Combine(parent!.PhysicalPath, dto.File.FileName); //the parent cannot be null.
            var filePath = Path.Combine(_updatePath, relativePath);
            version = await _documentRepository.GetDocumentVersionNumberAsync(parent!.Id);



            var file = new DocumentVersion
            {
                Id = Guid.NewGuid(),
                DocumentId = dto.ParentId,
                PhysicalPath = relativePath,
                Extension = "none",
                ChangedBy = dto.UpdatedBy,
                ChangeDescription = dto.Reason,
                VersionNumber = version + 1 //shows the next version value to be added
            };

            var value = await _documentRepository.AddVersionAsync(file);
            if (value < 1)
            {
                return "Could not be updated.";
            }

            var dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir!);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }
            return "Saved successfully.";

        }

        public async Task<List<DocumentVersion>> FetchAllVersionsAsync(Guid id)
        {
            var versions = await _documentRepository.FetchAllVersionsAsync(id);
            return versions;
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

        public async Task<(DocumentVersion? Meta, string? FilePath)> GetDocumentVersionWithFileAsync(Guid id)
        {
            var doc = await _documentRepository.GetVersionByIdAsync(id);
            if (doc == null) return (null, null);

            string filepath = "uploads/updates" + doc.PhysicalPath;
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
            var (form1, fields1) = await _documentRepository.CreateFormWithFieldsAsync(form, fields);
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

        public async Task<string> SubmitFormResponseAsync(FormTemplateDTO dto)
        {
            await Task.Delay(1000);
            return "Form response submitted successfully";
        }


        public static string ExtractText(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            return extension switch
            {
                ".pdf"  => ExtractPdf(filePath),
                ".docx" => ExtractWord(filePath),
                _       => throw new NotSupportedException($"File type {extension} is not supported.")
            };
        }

        private static string ExtractPdf(string filePath)
        {
            var sb = new StringBuilder();

            using (var pdf = PdfDocument.Open(filePath))
            {
                foreach (var page in pdf.GetPages())
                {
                    sb.AppendLine(page.Text);
                }
            }

            return sb.ToString();
        }

        private static string ExtractWord(string filePath)
        {
            var sb = new StringBuilder();

            using (var wordDoc = WordprocessingDocument.Open(filePath, false))
            {
                var body = wordDoc.MainDocumentPart?.Document.Body;
                if (body != null)
                {
                    sb.Append(body.InnerText);
                }
            }

            return sb.ToString();
        }


    }


    public class DocumentStorageOptions
    {
        public string BasePath { get; set; } = string.Empty;
    }



}