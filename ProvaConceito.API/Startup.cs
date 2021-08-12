using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProvaConceito.DataAccess.Data;
using ProvaConceito.DataAccess.Services;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProvaConceito.API
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
            services.AddScoped<EscolaService>();
            services.AddScoped<AlunoService>();
            services.AddScoped<TurmaService>();
            services.AddScoped<ProfessorService>();
            services.AddDbContext<ProvaConceitoContext>(options => options.UseSqlite(Configuration.GetConnectionString("SqliteConnection")));
            services.AddControllers(options =>
            {
                options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
                options.OutputFormatters.Add(
                    new SystemTextJsonOutputFormatter(
                        new JsonSerializerOptions(JsonSerializerDefaults.Web)
                        {
                            ReferenceHandler = ReferenceHandler.Preserve,
                        }));
            });

            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProvaConceito API V1");
                c.RoutePrefix = "api";
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
