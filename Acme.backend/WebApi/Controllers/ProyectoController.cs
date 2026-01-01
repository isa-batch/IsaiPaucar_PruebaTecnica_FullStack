using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.DTO.Proyecto;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProyectoController : ControllerBase
    {
        private readonly ProyectoDomain _proyectoDomain;

        public ProyectoController(ProyectoDomain proyectoDomain)
        {
            _proyectoDomain = proyectoDomain;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _proyectoDomain.GetAllAsync();

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _proyectoDomain.GetByIdAsync(id);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProyectoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _proyectoDomain.CreateAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProyectoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _proyectoDomain.UpdateAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _proyectoDomain.DeleteAsync(id);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // ===== INVITACIONES =====

        [HttpPost("invitar")]
        public async Task<IActionResult> InvitarUsuario([FromBody] InvitacionRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _proyectoDomain.InvitarUsuarioAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("mis-invitaciones")]
        public async Task<IActionResult> GetMisInvitaciones()
        {
            var response = await _proyectoDomain.GetMisInvitacionesAsync();

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("invitaciones/{invitacionId}/aceptar")]
        public async Task<IActionResult> AceptarInvitacion(int invitacionId)
        {
            var response = await _proyectoDomain.ResponderInvitacionAsync(invitacionId, true);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("invitaciones/{invitacionId}/rechazar")]
        public async Task<IActionResult> RechazarInvitacion(int invitacionId)
        {
            var response = await _proyectoDomain.ResponderInvitacionAsync(invitacionId, false);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
