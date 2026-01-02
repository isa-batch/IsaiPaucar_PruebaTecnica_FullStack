using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(
            this IServiceCollection services)
        {
            // Repositorios
            services.AddScoped<Interfaces.IUsuarioRepository, Implementations.UsuarioRepository>();
            services.AddScoped<Interfaces.IProyectoRepository, Implementations.ProyectoRepository>();
            services.AddScoped<Interfaces.ITareaRepository, Implementations.TareaRepository>();

            return services;
        }
    }
}
