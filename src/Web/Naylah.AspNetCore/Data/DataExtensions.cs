using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Naylah.Data
{
    public static class DataExtensions
    {
        public static IServiceCollection AddDataManagement(this IServiceCollection services,
             Action<MvcOptions> mvcCoreOptions = null,
             Action<SwaggerGenOptions> swaggerOptions = null
         )
        {
            //services.AddOData();

            services.AddMvcCore(options =>
            {

                // Workaround: https://github.com/OData/WebApi/issues/1177
                //options.OutputFormatters.OfType<ODataOutputFormatter>().ToList().ForEach(x =>
                //{
                //    options.OutputFormatters.Remove(x);
                //});

                //options.InputFormatters.OfType<ODataInputFormatter>().ToList().ForEach(x =>
                //{
                //    options.InputFormatters.Remove(x);
                //});

                //foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                //{
                //    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
                //}
                //foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                //{
                //    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
                //}

                mvcCoreOptions?.Invoke(options);
            });

            if (swaggerOptions != null)
            {
                services.AddSwaggerGen(c =>
                {
                    swaggerOptions?.Invoke(c);
                });
            }

            return services;
        }
    }
}
