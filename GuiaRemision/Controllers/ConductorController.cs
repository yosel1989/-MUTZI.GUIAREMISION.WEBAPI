using GuiaRemision.DTOs.GuiaRemitente.Conductor;
//using GuiaRemision.Services.Conductor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuiaRemision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConductorController : ControllerBase
    {
        //private readonly IConductorService _service;
        //public ConductorController(IConductorService service)
        //{
        //    _service = service;
        //}
        //[HttpGet]
        //[Route("listar-conductor")]
        //public async Task<ActionResult<List<ConductorDTO>>> GetAll()
        //{
        //    var result = await _service.GetAllAsync();
        //    return Ok(result);
        //}
        //[HttpGet]
        //[Route("obtener-conductor/{id}")]
        //public async Task<ActionResult<ConductorDTO>> GetById(int id)
        //{
        //    var result = await _service.GetByIdAsync(id);
        //    return Ok(result);
        //}
        //[HttpPost]
        //[Route("crear-conductor")]
        //public async Task<ActionResult<int>> Create(ConductorCreateDTO conductorDto)
        //{
        //    var newId = await _service.CreateAsync(conductorDto);
        //    return Ok(newId);
        //}
        //[HttpPut]
        //[Route("actualizar-conductor/{id}")]
        //public async Task<ActionResult<ConductorUpdateDTO>> Update(int id, ConductorUpdateDTO conductorDto)
        //{
        //    var updatedConductor = await _service.UpdateAsync(id, conductorDto);
        //    return Ok(updatedConductor);
        //}
        //[HttpDelete]
        //[Route("eliminar-conductor/{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _service.DeleteAsync(id);
        //    return NoContent();
        //}
    }
}
