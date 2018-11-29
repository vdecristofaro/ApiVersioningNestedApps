
using System;
using Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NestedApp1.Controllers;
using Newtonsoft.Json.Serialization;

namespace NestedApp1 {
    public class NestedStartup1 {
        private readonly IHostingEnvironment _hostingEnvironment;

        public NestedStartup1( IHostingEnvironment hostingEnvironment, IConfiguration configuration ) {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices( IServiceCollection services ) {
            services.AddTransient<IHelloService, FrenchHelloService>();
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
                        manager.FeatureProviders.Add( new TypedControllerFeatureProvider<NestedApp1Controller>() );
                    } );

            var assemblies = new System.Reflection.Assembly[] {
                typeof(NestedStartup1).Assembly,
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
