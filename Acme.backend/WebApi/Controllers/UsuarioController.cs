using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Repository.Interfaces;
using Model.Common;
using Model.DTO.Usuario;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> SearchUsuarios([FromQuery] string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
            {
                return Ok(Response<List<UsuarioDto>>.SuccessResponse(new List<UsuarioDto>()));
            }

            var usuarios = await _usuarioRepository.SearchAsync(termino);
            var usuariosDto = _mapper.Map<List<UsuarioDto>>(usuarios);

            return Ok(Response<List<UsuarioDto>>.SuccessResponse(usuariosDto));
        }
    }
}
