using System.ComponentModel.DataAnnotations;

namespace ExampleImageInAzureStorage.Core.Dto
{
    public class ExampleStorageDto
    {
        [Required]
        public string? Name { get; set; }
        public DateTime? DateCreated { get; set; }
        [Required]
        public IFormFile? Picture { get; set; }
    }
}
