using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using NestedApp;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace ApiHost {
    public class Startup {
        public Startup( IConfiguration configuration ) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices( IServiceCollection services ) {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddApiVersioning( o => {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.DefaultApiVersion = new ApiVersion( 1, 0 );
            } );

            services.AddHttpsRedirection( redirectOptions => {
                redirectOptions.RedirectStatusCode = StatusCodes.Status301MovedPermanently;
            } );

            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            services.AddMvcCore()
                    .SetCompatibilityVersion( CompatibilityVersion.Version_2_1 )
                    .AddApiExplorer()
                    .AddVersionedApiExplorer( o => o.GroupNameFormat = "'v'VVV" )
                    .AddJsonFormatters( jsonSettings => {
                        jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    } );

            services.AddSwaggerGen( swaggerOptions => {
                swaggerOptions
                    .SwaggerDoc( "v1", new Info { Title = "My API V1", Version = "v1" } );
                swaggerOptions
                    .SwaggerDoc( "v2", new Info { Title = "My API V2", Version = "v2" } );
            } );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure( IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider ) {
            if ( env.IsDevelopment() ) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI( swaggerUiOptions => {
                // build a swagger endpoint for each discovered API version
                foreach ( var description in provider.ApiVersionDescriptions ) {
                    swaggerUiOptions
                        .SwaggerEndpoint( $"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant() );
                }
            } );

            app.IsolatedMap<NestedStartup>( "/nested" );
        }

    }
}
