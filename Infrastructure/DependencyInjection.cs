using Domain.RepositoryInterface;
using Infrastructure.Database;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}