using CleanArchMvc.Application.Interfaces.Repositories;
using CleanArchMvc.Application.Interfaces.Services;
using CleanArchMvc.Application.Mappings;
using CleanArchMvc.Application.Services;
using CleanArchMvc.Infrastructure;
using CleanArchMvc.Infrastructure.Files;
using CleanArchMvc.Infrastructure.Identity;
using CleanArchMvc.Infrastructure.Repositories;
using CleanArchMvc.Infrastructure.Storage;
using CleanArchMvc.Infrastructure.Twilio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CleanArchMvc.Infra.IoC
{
    public static class DependencyInjectionAPI
    {
        public static IServiceCollection AddInfrastructureAPI(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterContext(services, configuration);

            RegisterRepositories(services, configuration);

            RegisterServices(services, configuration);

            RegisterAutoMapper(services, configuration);

            RegisterMediatR(services);

            RegisterIdentity(services);

            RegisterSMSOptions(services, configuration);

            return services;
        }

        private static void RegisterSMSOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TwilioOptions>(configuration);
        }

        private static void RegisterIdentity(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();
        }

        private static void RegisterMediatR(IServiceCollection services)
        {
            var handlers = AppDomain.CurrentDomain.Load("CleanArchMvc.Application");
            services.AddMediatR(handlers);
        }

        private static void RegisterAutoMapper(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(DomainToDTOMappingProfile));
            services.AddAutoMapper(typeof(DTOToCommandMappingProfile));
        }

        private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICurrentUser, CurrentUser>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddScoped<IAccountService, IdentityService>();
            services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFileService, CSVReader>();
            services.AddScoped<IFileService, ExcelReader>();

            services.AddScoped<ISMSService, TwilioService>();

            services.AddScoped<IStorageService, AwsS3Service>();
        }

        private static void RegisterRepositories(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }

        private static void RegisterContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }
    }
}