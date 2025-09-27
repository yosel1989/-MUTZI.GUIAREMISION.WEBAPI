using GuiaRemision.DTOs.GuiaRemitente.Proveedor;
using GuiaRemision.Services.GuiaRemitente.Proveedor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GuiaRemision.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly IProveedorService _service;
        public ProveedorController(IProveedorService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("listar")]
        public async Task<ActionResult<List<ProveedorDTO>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("buscar-por-id/{id}")]
        public async Task<ActionResult<ProveedorDTO>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }
        [HttpGet]
        [Route("buscar-por-numero-doc/{numeroDocumento}")]
        public async Task<ActionResult<ProveedorDTO>> GetByNumeroDocumento(string numeroDocumento)
        {
            var result = await _service.GetByNumeroDocumentoAsync(numeroDocumento);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<int>> Create(ProveedorCreateDTO proveedorDto)
        {
            var newId = await _service.CreateAsync(proveedorDto);
            return Ok(newId);
        }
        [HttpPut]
        public async Task<ActionResult<ProveedorUpdateDTO>> Update(int id, ProveedorUpdateDTO proveedorDto)
        {
            var updatedProveedor = await _service.UpdateAsync(id, proveedorDto);
            return Ok(updatedProveedor);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Ok($"Proveedor con ID: {id} eliminado.");
        }
    }
}
