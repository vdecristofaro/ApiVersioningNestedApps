
using System;
using Common;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NestedApp1.Controllers;
using Newtonsoft.Json.Serialization;

namespace NestedApp1 {
    public class NestedStartup1 {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IGlobalHelloService _globalHelloService;
        private readonly IApiDescriptionGroupCollectionProvider _apiDescriptionGroupCollectionProvider;

        public NestedStartup1( IHostingEnvironment hostingEnvironment, IConfiguration configuration,
                               IGlobalHelloService globalHelloService, IApiDescriptionGroupCollectionProvider apiDescriptionGroupCollectionProvider ) {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
            _globalHelloService = globalHelloService;
            _apiDescriptionGroupCollectionProvider = apiDescriptionGroupCollectionProvider;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices( IServiceCollection services ) {
            services.AddTransient<IHelloService, FrenchHelloService>();
            services.AddTransient<IGlobalHelloService>( provider => { return _globalHelloService; } );
            services.AddApiVersioning( options => {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion( 1, 0 );
                options.UseApiBehavior = true;
                options.ApiVersionReader = new HeaderApiVersionReader( "x-domec-api-version" );
                options.ApiVersionSelector = new LowestImplementedApiVersionSelector( options );
            } );

            services.AddMvcCore( options => {
            } )
            .SetCompatibilityVersion( CompatibilityVersion.Version_2_2 )
            //.AddApiExplorer()
            .AddJsonFormatters( jsonSettings => {
                jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            } )
            .ConfigureApplicationPartManager( manager => {
                manager.FeatureProviders.Clear();
                manager.FeatureProviders.Add( new TypedControllerFeatureProvider<NestedApp1Controller>() );
            } );
            services.TryAddEnumerable( ServiceDescriptor.Transient<IApiDescriptionProvider, DefaultApiDescriptionProvider>() );
            //services.AddVersionedApiExplorer( o => {
            //    o.GroupNameFormat = "'v'VVV";
            //    o.SubstituteApiVersionInUrl = true;
            //} );

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
