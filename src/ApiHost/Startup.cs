using ApiHost.Controllers;
using Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NestedApp1;
using NestedApp2;
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
            services.AddTransient<IHelloService, ItalianHelloService>();
            services.AddTransient<IGlobalHelloService, DefaultGlobalService>();

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
                    .SetCompatibilityVersion( CompatibilityVersion.Version_2_2 )
                    .AddApiExplorer()
                    .AddJsonFormatters( jsonSettings => {
                        jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    } )
                    .ConfigureApplicationPartManager( manager => {
                        manager.FeatureProviders.Clear();
                        manager.FeatureProviders.Add( new TypedControllerFeatureProvider<HostControllerBase>() );
                    } );
            services.AddVersionedApiExplorer( o => {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
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
            app.IsolatedMap<NestedStartup1>( "/v{version:apiVersion}/n1" );
            app.IsolatedMap<NestedStartup2>( "/n2" );
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
        }

    }
}
