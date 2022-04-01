namespace ExampleImageInAzureStorage.Infrastructure.Interfaz
{
    public interface ILocalhostStorageFile
    {
        Task DeleteFile(string path, string container);
        Task<string> EditFile(string container, IFormFile file, string path);
        Task<string> SaveFile(string container, IFormFile file);
    }
}
