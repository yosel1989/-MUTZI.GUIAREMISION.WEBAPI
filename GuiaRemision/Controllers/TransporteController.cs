using GuiaRemision.DTOs.GuiaRemitente.UnidadTransporte;
using GuiaRemision.Services.GuiaRemitente.Transporte;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuiaRemision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransporteController : ControllerBase
    {
        private readonly ITransporteService _service;
        public TransporteController(ITransporteService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<UnidadTransporteDTO>>> GetAll()
        {
            var result = await _service.GetAllAsycn();
            return Ok(result);
        }
        [HttpGet]
        [Route("buscar-por-id/{id}")]
        public async Task<ActionResult<UnidadTransporteDTO>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<int>> Create(UnidadTransporteCreateDTO transporteDto)
        {
            var newId = await _service.CreateAsync(transporteDto);
            return Ok(newId);
        }
        [HttpPut]
        public async Task<ActionResult<UnidadTransporteUpdateDTO>> Update(int id, UnidadTransporteUpdateDTO transporteDto)
        {
            var updatedTransporte = await _service.UpdateAsync(id, transporteDto);
            return Ok(updatedTransporte);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok($"Transporte con ID: {id} eliminado.");
        }
    }
}
