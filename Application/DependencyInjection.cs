using Application.Helpers;
using Domain.Models;
using FluentValidation;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<DatabaseContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<TokenHelper>();

            services.AddAutoMapper(assembly);

            services.AddValidatorsFromAssembly(assembly);

            return services;
        }
    }
}