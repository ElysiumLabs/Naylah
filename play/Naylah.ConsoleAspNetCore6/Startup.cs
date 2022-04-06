using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Naylah.Data;
using Naylah.Data.Access;
using Newtonsoft.Json.Serialization;

namespace Naylah.ConsoleAspNetCore
{
    public class StartupOptions : ServiceOptions
    {
        public StartupOptions()
        {
            
        }
    }

    public class Startup : Service<StartupOptions>
    {

        public Startup(IHostEnvironment environment, IConfiguration configuration) : base(environment, configuration)
        {
            Options.Name = "My awesome API";
        }

       
        protected override void ConfigureServicesApp(IServiceCollection services)
        {
            //services.AddAutoMapper(typeof(Startup));
            services.
                AddControllers().
                AddOData(options => 
                {
                    options.EnableQueryFeatures(1000); 
                });


            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            //        .AddNewtonsoftJson(options =>
            //        {
            //            options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            //            options.SerializerSettings.ContractResolver = new DefaultContractResolver
            //            {
            //                NamingStrategy = new CamelCaseNamingStrategy()
            //            };
            //        });



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


            //services.
            //    AddDbContext<ORM.TestDbContext>(options => options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=NaylahTestDevDB;Trusted_Connection=True;MultipleActiveResultSets=false", ORM.TestDbContext.ConfigureDBContext))
            //;

            //services.AddSingleton(new List<Person>());
            //services.AddScoped<IRepository<Person, string>, SomeRepository>();

            //services.AddEntityFrameworkRepository<ORM.TestDbContext, Person>();

            //services.AddSingleton<CosmosClient>(x =>
            //{
            //    var c = new CosmosClient(
            //        "",
            //        "",
            //        new CosmosClientOptions()
            //        {
            //            ApplicationName = Options.Name,
            //            AllowBulkExecution = false,

            //            SerializerOptions = new CosmosSerializationOptions()
            //            {
            //                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
            //            },
            //        });

            //    //var handler = new CustomHttpClientHandler();
            //    //c.ClientOptions.CustomHandlers.Add(handler);

            //    return c;
            //});

            //services.AddScoped<IRepository<Entities.Person>, CosmosSQLContainerRepository<Entities.Person>>(x =>
            //{
            //    return new CosmosSQLContainerRepository<Entities.Person>
            //    (
            //        x.GetService<CosmosClient>().GetContainer("", "test"),
            //        y => new PartitionKey(y.Partition)
            //    );
            //});

            //services.AddScoped(typeof(StringAppTableDataService<,>));
            //services.AddScoped<IUnitOfWork, SomeWorker>();

            //services.AddScoped<PersonService>();
            ////services.AddScoped<PersonServiceV2>();

            services.AddHealthChecks().
                AddCheck<TestCheck>("SqlDb").
                AddCheck<TestCheck>("CosmosDb").
                AddCheck<TestCheck>("SAPIntegration").
                AddCheck<TestCheck>("Teste3").
                AddCheck<TestCheck>("Teste4").
                AddCheck<TestCheck>("Teste5").
                AddCheck<TestCheck>("Teste6");

            services.AddScoped<string>((c) => Guid.NewGuid().ToString());


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        protected override void ConfigureApp(IApplicationBuilder app)
        {
            //app.UseBlockingDetection();

            //if (Environment.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapODataRoute("odata", "odata", GetEdmModel());
                endpoints.MapControllers();

                endpoints.MapHealthChecks("/health");
            });

            
        }

        IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            //odataBuilder.EntitySet<Person>("Person");

            return odataBuilder.GetEdmModel();
        }

    }

    public class TestCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            await Task.Delay(3000);
            var r = new Random();
            var i = r.Next(3);
            switch (i)
            {
                case 1: return HealthCheckResult.Healthy("");
                case 2: return HealthCheckResult.Degraded("");
                case 3: return HealthCheckResult.Unhealthy("");
                default: return HealthCheckResult.Unhealthy("");
            }

        }
    }
}
