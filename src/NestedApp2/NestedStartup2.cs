using System;
using System.Reflection;
using Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NestedApp2.Controllers;
using Newtonsoft.Json.Serialization;

namespace NestedApp2 {
    public class NestedStartup2 {
        private readonly IHostingEnvironment _hostingEnvironment;

        public NestedStartup2( IHostingEnvironment hostingEnvironment, IConfiguration configuration ) {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices( IServiceCollection services ) {
            services.AddTransient<IHelloService, EnglishHelloService>();

            services.AddApiVersioning( o => {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ReportApiVersions = true;
                o.DefaultApiVersion = new ApiVersion( 1, 0 );
            } );

            services.AddMvcCore()
                    .SetCompatibilityVersion( CompatibilityVersion.Version_2_1 )
                    .AddApiExplorer()
                    .AddVersionedApiExplorer( o => {
                        o.GroupNameFormat = "'v'VVV";
                        o.SubstituteApiVersionInUrl = true;
                    } )
                    .AddJsonFormatters( jsonSettings => {
                        jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    } )
                    .ConfigureApplicationPartManager( manager => {
                        manager.FeatureProviders.Clear();
                        manager.FeatureProviders.Add( new TypedControllerFeatureProvider<NestedApp2Controller>() );
                    } );
            var assemblies = new Assembly[] {
                typeof(NestedStartup2).Assembly,
                typeof(IHelloService).Assembly
            };
            services.AddMediatR( assemblies );
            services.AddTransient( typeof( IPipelineBehavior<,> ), typeof( PipelineBehavior1<,> ) );
        }

        public void Configure( IApplicationBuilder app, IHostingEnvironment env ) {
            app.UseMvc();
        }
    }
}
