using System;
using System.ComponentModel.DataAnnotations;

namespace ExampleImageInAzureStorage.Core.Entities
{
    public class ExampleStorage
    {
        [Key]
        public int IdStorageFile { get; set; }
        public string? Name { get; set; }
        public DateTime? DateCreated { get; set; }
        public string? Picture { get; set; }
    }
}
