using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Naylah.ConsoleAspNetCore.Customizations;
using Naylah.ConsoleAspNetCore.Entities;
using Naylah.Data;
using Naylah.Data.Access;
using Newtonsoft.Json.Serialization;

namespace Naylah.ConsoleAspNetCore
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy()
                        };
                    });

            services.AddDataManagement(null, swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "teste" + " " + " API", Version = "v1" });

                var scheme1 = new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert Bearer authorization into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                };

                swagger.AddSecurityDefinition("Bearer", scheme1);


                //swagger.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                //{
                //    In = "header",
                //    Description = "Please insert APIKEY into field",
                //    Name = "x-api-key",
                //    Type = "apiKey"
                //});

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {

                });

                //var requiriment = new OpenApiSecurityRequirement();
                //requiriment.Add(scheme1, );
                //swagger.AddSecurityRequirement(requiriment);

            });


            services.
                AddDbContext<ORM.TestDbContext>(options => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=NaylahTestDevDB;Trusted_Connection=True;MultipleActiveResultSets=false", ORM.TestDbContext.ConfigureDBContext))
            ;

            //services.AddSingleton(new List<Person>());
            //services.AddScoped<IRepository<Person, string>, SomeRepository>();

            services.AddEntityFrameworkRepository<ORM.TestDbContext, Person, string>();

            services.AddScoped(typeof(StringAppTableDataService<,>));
            services.AddScoped<IUnitOfWork, SomeWorker>();

            services.AddScoped<PersonService>();
            services.AddScoped<PersonServiceV2>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.EnableDependencyInjection();
            });

        }
    }
}
