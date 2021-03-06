﻿using System;
using System.Reflection;
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
using NestedApp2.Controllers;
using Newtonsoft.Json.Serialization;

namespace NestedApp2 {
    public class NestedStartup2 {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IGlobalHelloService _globalHelloService;

        public NestedStartup2( IHostingEnvironment hostingEnvironment, IConfiguration configuration, IGlobalHelloService globalHelloService ) {
            _hostingEnvironment = hostingEnvironment;
            Configuration = configuration;
            _globalHelloService = globalHelloService;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices( IServiceCollection services ) {
            services.AddTransient<IHelloService, EnglishHelloService>();
            services.AddTransient<IGlobalHelloService>( provider => { return _globalHelloService; } );

            services.AddApiVersioning( options => {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion( 3, 0 );
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
                manager.FeatureProviders.Add( new TypedControllerFeatureProvider<NestedApp2Controller>() );
            } );
            services.TryAddEnumerable( ServiceDescriptor.Transient<IApiDescriptionProvider, DefaultApiDescriptionProvider>() );

            //services.AddVersionedApiExplorer( o => {
            //    o.GroupNameFormat = "'v'VVV";
            //    o.SubstituteApiVersionInUrl = true;
            //} );

            var assemblies = new Assembly[] {
                typeof(NestedStartup2).Assembly,
                typeof(IHelloService).Assembly
            };
            services.AddMediatR( assemblies );
            services.AddTransient( typeof( IPipelineBehavior<,> ), typeof( PipelineBehavior2<,> ) );
        }

        public void Configure( IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider ) {
            app.UseMvc();
        }
    }
}
