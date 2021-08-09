using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExampleImageInAzureStorage.Core.Dto
{
    public class ExampleStorageDto
    {
        [Required]
        public string Name { get; set; }

        public DateTime? DateCreated { get; set; }

        public IFormFile Picture { get; set; }
    }
}
