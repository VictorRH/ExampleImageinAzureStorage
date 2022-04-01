using ExampleImageInAzureStorage.Core.Persistence;
using ExampleImageInAzureStorage.Infrastructure.Interfaz;
using ExampleImageInAzureStorage.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NetTopologySuite;
using NetTopologySuite.Geometries;

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
            //services.AddSingleton(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));
            services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

            services.AddDbContext<StorageAzureContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("connectionDB"));
            });
            services.AddControllers();
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Example Save Image in Azure Storage",
                    Version = "v1"
                });
            });
            services.AddCors(o => o.AddPolicy("corsAPP", builder =>
            {
                builder.WithOrigins("*").
                        AllowAnyMethod().
                        AllowAnyHeader();
            }));

            //services.AddCors(opciones =>
            //{
            //    opciones.AddDefaultPolicy(builder =>
            //    {
            //        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            //    });
            //});


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("v1/swagger.json", "Example Save Image in Azure Storage");
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("corsAPP");
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
