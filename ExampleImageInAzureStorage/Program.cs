using ExampleImageInAzureStorage.Core.Persistence;
using ExampleImageInAzureStorage.Infrastructure.Interfaz;
using ExampleImageInAzureStorage.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(o => o.AddPolicy("corsAPP", builder =>
{
    builder.WithOrigins("*").
            AllowAnyMethod().
            AllowAnyHeader();
}));
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAzureStorageFile, AzureStorage>();
//this is for localhost
builder.Services.AddTransient<ILocalhostStorageFile, LocalStorageFile>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddSingleton(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
builder.Services.AddDbContext<StorageAzureContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB"));
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();
app.UseCors("corsAPP");
app.Run();
