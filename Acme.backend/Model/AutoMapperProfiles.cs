using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Aquí defines tus mapeos
            // Ejemplo básico (cuando crees entidades y DTOs):

            // CreateMap<Producto, ProductoDTO>();
            // CreateMap<ProductoDTO, Producto>();

            // CreateMap<Usuario, UsuarioDTO>();
            // CreateMap<UsuarioDTO, Usuario>();

            // Mapeo bidireccional automático:
            // CreateMap<Producto, ProductoDTO>().ReverseMap();
        }
    }
}
