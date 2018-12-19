
using System;
using Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NestedApp1.Controllers;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace NestedApp1 {
    public class NestedStartup1 {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IGlobalHelloService _globalHelloService;

        public NestedStartup1( IHostingEnvironment hostingEnvironment, IConfiguration configuration, IGlobalHelloService globalHelloService ) {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
            _globalHelloService = globalHelloService;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices( IServiceCollection services ) {
            services.AddTransient<IHelloService, FrenchHelloService>();
            services.AddTransient<IGlobalHelloService>( provider => { return _globalHelloService; } );
            services.AddApiVersioning( o => {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.DefaultApiVersion = new ApiVersion( 1, 0 );
            } );

            services.AddMvcCore()
                    .SetCompatibilityVersion( CompatibilityVersion.Version_2_1 )
                    .AddApiExplorer()
                    .AddJsonFormatters( jsonSettings => {
                        jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    } )
                    .ConfigureApplicationPartManager( manager => {
                        manager.FeatureProviders.Clear();
                        manager.FeatureProviders.Add( new TypedControllerFeatureProvider<NestedApp1Controller>() );
                    } );
            services.AddVersionedApiExplorer( o => {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            } );

            var assemblies = new System.Reflection.Assembly[] {
                typeof(NestedStartup1).Assembly,
                typeof(IHelloService).Assembly
            };
            services.AddMediatR( assemblies );
            services.AddTransient( typeof( IPipelineBehavior<,> ), typeof( PipelineBehavior1<,> ) );

            services.AddSwaggerGen( swaggerOptions => {
                swaggerOptions
                    .SwaggerDoc( "v1", new Info { Title = "My API V1", Version = "v1" } );
                swaggerOptions
                    .SwaggerDoc( "v2", new Info { Title = "My API V2", Version = "v2" } );
            } );
        }

        public void Configure( IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider ) {
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI( swaggerUiOptions => {
                // build a swagger endpoint for each discovered API version
                foreach ( var description in provider.ApiVersionDescriptions ) {
                    swaggerUiOptions
                        .SwaggerEndpoint( $"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant() );
                }
            } );
        }
    }
}
