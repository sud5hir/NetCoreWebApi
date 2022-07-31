using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
using Microsoft.EntityFrameworkCore;
using QueryDataApi.Respository;
using QueryDataApi.Dal;
using QueryDataApi.Model;

namespace WebApplication1
{
    public class Startup
    {
        private readonly string _policyName = "CorsPolicy";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
              {
                  c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
              });
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();
            services.AddCors(opt =>
            {
                opt.AddPolicy(name: _policyName, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddScoped<IRepository<QueryModel>, Repository<QueryModel>>();
            services.AddDbContext<QueryContextDal>(context => { context.UseInMemoryDatabase("ConferencePlanner"); });

            services.AddControllers().AddNewtonsoftJson();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            using (var sc = app.ApplicationServices.CreateScope())
            {
                var context = sc.ServiceProvider.GetService<QueryContextDal>();
                GetQuries(context);
                context.SaveChanges();
            }

            app.UseCors(_policyName);
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void GetQuries(QueryContextDal contextDal)
        {
            var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"data\\{"Query.json"}");

            var JSON = System.IO.File.ReadAllText(folderDetails);

            var quries = Newtonsoft.Json.JsonConvert.DeserializeObject<Queries>(JSON);

            contextDal.Quries.AddRange(quries.QueryList);
        }

    }
}
