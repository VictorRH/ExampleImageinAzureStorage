using AutoMapper;
using ExampleImageInAzureStorage.Core.Dto;
using ExampleImageInAzureStorage.Core.Entities;
using ExampleImageInAzureStorage.Core.Persistence;
using ExampleImageInAzureStorage.Infrastructure.Interfaz;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExampleImageInAzureStorage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AzureStorageController : ControllerBase
    {
        private readonly IAzureStorageFile azureStorage;
        private readonly ILocalhostStorageFile localhost;
        private readonly IMapper mapper;
        private readonly StorageAzureContext context;
        private readonly string container = "example";
        public AzureStorageController(IAzureStorageFile azureStorage, ILocalhostStorageFile localhost, IMapper mapper, StorageAzureContext context)
        {
            this.azureStorage = azureStorage;
            this.localhost = localhost;
            this.mapper = mapper;
            this.context = context;
        }
        [HttpPost("azure")]
        public async Task<ActionResult> PostImageAzure([FromForm] ExampleStorageDto storageDto)
        {
            var exampleFile = mapper.Map<ExampleStorage>(storageDto);
            if (storageDto.Picture != null)
            {
                exampleFile.Picture = await azureStorage.SaveFile(container, storageDto.Picture);
            }
            exampleFile.DateCreated = DateTime.UtcNow;
            context.Add(exampleFile);
            var response = await context.SaveChangesAsync();
            if (response > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
        [EnableCors("corsAPP")]
        [HttpPost("localhost")]
        public async Task<ActionResult> PostImageLocalhost([FromForm] ExampleStorageDto storageDto)
        {
            var exampleFile = mapper.Map<ExampleStorage>(storageDto);
            if (storageDto.Picture != null)
            {
                exampleFile.Picture = await localhost.SaveFile(container, storageDto.Picture);
            }
            exampleFile.DateCreated = DateTime.UtcNow;
            context.Add(exampleFile);
            var response = await context.SaveChangesAsync();
            if (response > 0)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> PutImageAzure(int id, [FromForm] ExampleStorageDto storageDto)
        {
            var storage = await context.ExampleStorage.FirstOrDefaultAsync(x => x.IdStorageFile == id);
            if (storage == null)
            {
                return NotFound();
            }
            storage = mapper.Map(storageDto, storage);
            if (storageDto.Picture != null)
            {
                storage.Picture = await azureStorage.EditFile(container, storageDto.Picture, storage.Picture!);
            }
            storage.DateCreated = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteImageAzureStorage(int id)
        {
            var storage = await context.ExampleStorage.FirstOrDefaultAsync(x => x.IdStorageFile == id);
            if (storage == null)
            {
                return NotFound();
            }
            context.Remove(storage);
            await context.SaveChangesAsync();
            await azureStorage.DeleteFile(storage.Picture!, container);
            return Ok();
        }
        [HttpGet]
        public async Task<ActionResult> AllImages()
        {
            var storage = await context.ExampleStorage.ToListAsync();
            if (storage == null)
            {
                return NotFound();
            }
            return Ok(storage);
        }
    }
}
