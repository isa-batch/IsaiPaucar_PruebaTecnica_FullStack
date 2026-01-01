using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Helpers de infraestructura
            services.AddScoped<Helpers.JwtTokenGenerator>();
            services.AddScoped<Helpers.ClaimsHelper>();

            return services;
        }
    }
}
