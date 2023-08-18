using System.Reflection.Metadata.Ecma335;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    //la keyword static significa che posso usare i metodi all'interno di
    //questa classe senza doverla istanziare ovviamente i metodi qui dentro
    //saranno tutti statici

    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();

            services.AddScoped<ITokenService, TokenServices>();

            return services;
        }
    }
}