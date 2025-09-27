using GuiaRemision.DTOs.GuiaRemitente.Conductor;
using GuiaRemision.Services.GuiaRemitente.Conductor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuiaRemision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConductorController : ControllerBase
    {
        private readonly IConductorService _service;
        public ConductorController(IConductorService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<ConductorDTO>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("buscar-por-id/{id}")]
        public async Task<ActionResult<ConductorDTO>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<int>> Create(ConductorCreateDTO conductorDto)
        {
            var newId = await _service.CreateAsync(conductorDto);
            return Ok(newId);
        }
        [HttpPut]
        public async Task<ActionResult<ConductorUpdateDTO>> Update(int id, ConductorUpdateDTO conductorDto)
        {
            var updatedConductor = await _service.UpdateAsync(id, conductorDto);
            return Ok(updatedConductor);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok($"Conductor con ID: {id} eliminado.");
        }
    }
}
