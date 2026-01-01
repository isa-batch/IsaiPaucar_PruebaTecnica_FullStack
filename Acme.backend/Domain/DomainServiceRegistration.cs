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
            // Aquí se registran dominios
            // services.AddScoped<IProductoDomain, ProductoDomain>();

            return services;
        }
    }
}
