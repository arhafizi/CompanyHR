﻿using CompanyHR.Formaters;
using Contracts;
using LoggerService;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using Service;
using Services.Contracts;
using Microsoft.AspNetCore.Mvc.Versioning;
using CompanyHR.Presentation.Controllers;
using Marvin.Cache.Headers;

namespace CompanyHR.Extensions;
public static class ServiceExtensions {
    public static void ConfigureCors(this IServiceCollection services) =>
        services.AddCors(options => {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .WithExposedHeaders("X-Pagination"));
        });

    public static void ConfigureIISIntegration(this IServiceCollection services) =>
        services.Configure<IISOptions>(options => { });

    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddSingleton<ILoggerManager, LoggerManager>();
    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
    public static void ConfigureServiceManager(this IServiceCollection services) =>
        services.AddScoped<IServiceManager, ServiceManager>();
    public static IMvcBuilder AddCustomCSVFormatter(this IMvcBuilder builder) =>
        builder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));

    public static void AddCustomMediaTypes(this IServiceCollection services) {
        services.Configure<MvcOptions>(config => {
            var systemTextJsonOutputFormatter = config.OutputFormatters
            .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();
            if (systemTextJsonOutputFormatter != null) {
                systemTextJsonOutputFormatter.SupportedMediaTypes
                .Add("application/vnd.myapi.hateoas+json");
                systemTextJsonOutputFormatter.SupportedMediaTypes
                .Add("application/vnd.myapi.apiroot+json");
            }
            var xmlOutputFormatter = config.OutputFormatters
            .OfType<XmlDataContractSerializerOutputFormatter>()?
            .FirstOrDefault();
            if (xmlOutputFormatter != null) {
                xmlOutputFormatter.SupportedMediaTypes
                .Add("application/vnd.myapi.hateoas+xml");
                xmlOutputFormatter.SupportedMediaTypes
                .Add("application/vnd.myapi.apiroot+json");
            }
        });
    }
    public static void ConfigureVersioning(this IServiceCollection services) {
        services.AddApiVersioning(opt => {
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.DefaultApiVersion = new ApiVersion(1, 0);
            opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            //second option:
            //opt.ApiVersionReader = new QueryStringApiVersionReader("api-version");

            /*
             *if we have a lot of versions of a single controller, we can assign these
             *versions in the configuration instead & remove [ApiVersion] attr from the controllers : 
            opt.Conventions.Controller<CompaniesController>()
                .HasApiVersion(new ApiVersion(1, 0));
            opt.Conventions.Controller<CompaniesV2Controller>()
                .HasDeprecatedApiVersion(new ApiVersion(2, 0));
            */
        });
    }
    public static void ConfigureResponseCaching(this IServiceCollection services) =>
        services.AddResponseCaching();
    public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
        services.AddHttpCacheHeaders((expirationOpt) => {
            expirationOpt.MaxAge = 65;
            expirationOpt.CacheLocation = CacheLocation.Private;
        }, (validationOpt) => {
            validationOpt.MustRevalidate = true;
        });

}

