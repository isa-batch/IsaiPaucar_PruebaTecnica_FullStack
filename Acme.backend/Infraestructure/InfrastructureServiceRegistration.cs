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
            // Aquí registrarás servicios de infraestructura
            // Ejemplo:
            // services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            // services.AddScoped<IClaimsHelper, ClaimsHelper>();

            // Servicios externos (APIs de terceros)
            // services.AddHttpClient<IEmailService, EmailService>();

            return services;
        }
    }
}
