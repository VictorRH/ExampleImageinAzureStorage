using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleImageInAzureStorage.Core.Entities
{
    public class ExampleStorage
    {
        [Key]
        public int IdStorageFile { get; set; }

        public string Name { get; set; }
        public DateTime? DateCreated { get; set; }

        public string Picture { get; set; }
    }
}
