using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleImageInAzureStorage.Infrastructure.Interfaz
{
    public interface IAzureStorageFile
    {
        Task DeleteFile(string path, string container);
        Task<string> EditFile(string container, IFormFile file, string path);
        Task<string> SaveFile(string container, IFormFile file);
    }
}
