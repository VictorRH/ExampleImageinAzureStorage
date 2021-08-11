using ExampleImageInAzureStorage.Core.Persistence;
using ExampleImageInAzureStorage.Infrastructure.Interfaz;
using ExampleImageInAzureStorage.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
namespace ExampleImageInAzureStorage
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //this is for azure storage
            services.AddTransient<IAzureStorageFile, AzureStorage>();
            //this is for localhost
            services.AddTransient<ILocalhostStorageFile, LocalStorageFile>();

            services.AddAutoMapper(typeof(Startup));

            services.AddHttpContextAccessor();

            services.AddSingleton(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

            services.AddCors(o => o.AddPolicy("corsAPP", builder =>
            {
                builder.WithOrigins("*").
                        AllowAnyMethod().
                        AllowAnyHeader();
            }));

            services.AddDbContext<StorageAzureContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("connectionDB"));
            });
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Example Save Image in Azure Storage",
                    Version = "v1"
                });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors("corsAPP");

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("v1/swagger.json", "Example Save Image in Azure Storage");
            });
        }
    }
}
