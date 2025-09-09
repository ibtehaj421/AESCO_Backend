using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ASCO.Models;

namespace ASCO.DTOs
{
    public class CreateDocDto
    {
        public string Name { get; set; } = string.Empty;

        public Guid? ParentId { get; set; }

        public bool IsFolder { get; set; }

        public int CreatedBy { get; set; } //if the folder entity is false this information will need to be stored.

        // Only for files
        public IFormFile? File { get; set; }
    }

    // For updating metadata (not file contents)
    public class UpdateDocDto
    {
        public string? Name { get; set; }

        public Guid ParentId { get; set; }

        public int UpdatedBy { get; set; } //will be the person who updated the document.

        public IFormFile File { get; set; } = default!;

        public string? Reason { get; set; } 
    }

    // For returning document details (response)
    public class DocDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsFolder { get; set; }

        public Guid? ParentId { get; set; }

        public string? Path { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? CreatedBy { get; set; } //id of the person who created the document

        public DateTime? UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; } //id of the user updating the document.

        public List<DocDto> Children { get; set; } = new List<DocDto>(); // for hierarchy display
    }

    public class CreateFolderDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }

        public int createdby { get; set; }
        public int updatedby { get; set; }
    }

    public class UploadFileDto
    {
        public Guid? ParentId { get; set; }
        public IFormFile File { get; set; } = default!;

        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }

        public List<int> Users { get; set; } = new List<int>();
    }

    public static class DocumentMapper
    {
        public static DocDto ToDto(this Document doc)
        {
            return new DocDto
            {
                Id = doc.Id,
                Name = doc.Name,
                Path = doc.PhysicalPath,
                IsFolder = doc.IsFolder,
                ParentId = doc.ParentId,
                CreatedAt = doc.CreatedAt,
                UpdatedAt = doc.UpdatedAt
            };
        }
    }
}