using ExampleImageInAzureStorage.Infrastructure.Interfaz;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ExampleImageInAzureStorage.Infrastructure.Repository
{
    public class LocalStorageFile : ILocalhostStorageFile
    {
        private readonly IWebHostEnvironment enviroment;
        private readonly IHttpContextAccessor httpContextAccessor;
        public LocalStorageFile(IWebHostEnvironment enviroment, IHttpContextAccessor httpContextAccessor)
        {
            this.enviroment = enviroment;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Task DeleteFile(string path, string container)
        {
            if (string.IsNullOrEmpty(path))
            {
                return Task.CompletedTask;
            }

            var nameFile = Path.GetFileName(path);
            var directoryFile = Path.Combine(enviroment.WebRootPath, container, nameFile);
            if (File.Exists(directoryFile))
            {
                File.Delete(directoryFile);
            }

            return Task.CompletedTask;
        }

        public async Task<string> EditFile(string container, IFormFile file, string path)
        {
            await DeleteFile(path, container);
            return await SaveFile(container, file);
        }

        public async Task<string> SaveFile(string container, IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var nameFile = $"{Guid.NewGuid()}{extension}";

            string folder = Path.Combine(enviroment.WebRootPath, container);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string path = Path.Combine(folder, nameFile);

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                await File.WriteAllBytesAsync(path, content);
            }

            var urlCurrent = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}";
            var pathDB = Path.Combine(urlCurrent, container, nameFile).Replace("\\", "/");

            return pathDB;
        }
    }
}
