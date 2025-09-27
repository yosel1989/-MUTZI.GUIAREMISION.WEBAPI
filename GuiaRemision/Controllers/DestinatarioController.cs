using GuiaRemision.DTOs.GuiaRemitente.Destinatario;
using GuiaRemision.Services.GuiaRemitente.Destinatario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuiaRemision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DestinatarioController : ControllerBase
    {
        private readonly IDestinatarioService _destinatarioService;
        public DestinatarioController(IDestinatarioService destinatarioService)
        {
            _destinatarioService = destinatarioService;
        }
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<DestinatarioDTO>>> GetAll()
        {
            var destinatarios = await _destinatarioService.GetAllAsync();
            return Ok(destinatarios);
        }
        [HttpGet]
        [Route("buscar-por-id")]
        public async Task<ActionResult<DestinatarioDTO>> GetById(int id)
        {
            var destinatario = await _destinatarioService.GetByIdAsync(id);
            if (destinatario == null)
            {
                return NotFound($"Destinatario con ID {id} no encontrado.");
            }
            return Ok(destinatario);
        }
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] DestinatarioCreateDTO createDTO)
        {
            if (createDTO == null)
            {
                return BadRequest("El destinatario no puede ser nulo.");
            }
            var createdId = await _destinatarioService.CreateAsync(createDTO);
            return Ok(createdId);
        }
        [HttpPut]
        public async Task<ActionResult<DestinatarioUpdateDTO>> Update(int id, [FromBody] DestinatarioUpdateDTO updateDTO)
        {
            var updated = await _destinatarioService.UpdateAsync(id, updateDTO);
            return Ok(updated);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _destinatarioService.DeleteAsync(id);
            return Ok($"Destinatario con ID: {id} eliminado.");
        }
    }
}
