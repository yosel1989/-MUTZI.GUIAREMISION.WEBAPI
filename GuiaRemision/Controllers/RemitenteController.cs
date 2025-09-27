using GuiaRemision.DTOs.GuiaRemitente.Remitente;
using GuiaRemision.Services.GuiaRemision;
using GuiaRemision.Services.GuiaRemitente.Remitente;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuiaRemision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemitenteController : ControllerBase
    {
        private readonly IRemitenteService _remitenteService;
        public RemitenteController(IRemitenteService remitenteService)
        {
            _remitenteService = remitenteService;
        }
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<RemitenteDTO>>> GetAll()
        {
            var remitentes = await _remitenteService.GetAllAsync();
            return Ok(remitentes);
        }
        [HttpGet]
        [Route("buscar-por-id")]
        public async Task <ActionResult<RemitenteDTO>> GetById(int id)
        {
            var remitente = await _remitenteService.GetByIdAsync(id);
            if (remitente == null)
            {
                return NotFound($"Remitente con ID {id} no encontrado.");
            }
            return Ok(remitente);
        }
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] RemitenteCreateDTO createDTO)
        {
            if (createDTO == null)
            {
                return BadRequest("El remitente no puede ser nulo.");
            }
            var createdId = await _remitenteService.CreateAsync(createDTO);
            return Ok(createdId);
        }
        [HttpPut]
        public async Task<ActionResult<RemitenteUpdateDTO>> Update(int id, [FromBody] RemitenteUpdateDTO updateDTO)
        {
            var updated=await _remitenteService.UpdateAsync(id, updateDTO);
            return Ok(updated);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _remitenteService.DeleteAsync(id);
            return Ok($"Remitente con ID: {id} eliminado.");
        }
    }
}
