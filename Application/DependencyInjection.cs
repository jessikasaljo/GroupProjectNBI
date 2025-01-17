using Application.Helpers;
using Domain.RepositoryInterface;
using Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
            services.AddScoped<TokenHelper>();
            services.AddScoped(typeof(IVerificationService<>), typeof(VerificationService<>));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}