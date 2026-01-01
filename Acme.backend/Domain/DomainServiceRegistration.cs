using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class DomainServiceRegistration
    {
        public static IServiceCollection AddDomainServices(
            this IServiceCollection services)
        {
            // Dominios de negocio
            services.AddScoped<AuthDomain>();
            services.AddScoped<ProyectoDomain>();

            return services;
        }
    }
}
