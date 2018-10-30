
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NestedApp.Controllers;
using Newtonsoft.Json.Serialization;

namespace NestedApp {
    public class NestedStartup {
        private readonly IHostingEnvironment _hostingEnvironment;

        public NestedStartup( IHostingEnvironment hostingEnvironment, IConfiguration configuration ) {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices( IServiceCollection services ) {
            services.AddApiVersioning( o => {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.DefaultApiVersion = new ApiVersion( 1, 0 );
            } );

            services.AddMvcCore()
                    .SetCompatibilityVersion( CompatibilityVersion.Version_2_1 )
                    .AddApiExplorer()
                    .AddVersionedApiExplorer( o => o.GroupNameFormat = "'v'VVV" )
                    .AddJsonFormatters( jsonSettings => {
                        jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    } );

        }

        public void Configure( IApplicationBuilder app, IHostingEnvironment env ) {
            app.UseMvc();
        }
    }
}
