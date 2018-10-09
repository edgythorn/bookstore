using BooksStore.Host.Middleware;
using BooksStore.Host.SampleData;
using BooksStore.Interfaces;
using BooksStore.Services;
using BooksStore.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BooksStore.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _environment;

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvcCore()
                .AddJsonFormatters()
                .AddCors()
                .AddDataAnnotations()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddSingleton(sp =>
            {
                var settings = new BooksServiceSettings();
                Configuration.GetSection(typeof(BooksServiceSettings).Name).Bind(settings);
                settings.ImagesRootPath = _environment.WebRootPath;
                return settings;
            });

            services.AddSingleton<IBooksRepository, InMemoryBooksRepository>();
            services.AddTransient<IBooksService, BooksService>();
        }

        public void Configure(IApplicationBuilder app, IBooksRepository repository)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });

            app.UseStaticFiles();
            app.UseMvc();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (_environment.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            SampleDataInitializer.LoadDataFromJson(repository);
        }
    }
}
