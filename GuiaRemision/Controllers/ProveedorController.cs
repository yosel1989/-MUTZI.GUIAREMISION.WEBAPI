using GuiaRemision.DTOs.Efact;
using GuiaRemision.DTOs.GuiaRemitente.Destinatario;
using GuiaRemision.DTOs.GuiaRemitente.Proveedor;
using GuiaRemision.Services.GuiaRemitente.Proveedor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(
        Summary = "Listar proveedores",
        Description = "Lista los proveedores",
        OperationId = "listar_proveedor")]
        [ProducesResponseType(typeof(ProveedorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ProveedorDTO>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                if (result == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = "No se pudo obtener la lista de proveedores."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar proveedores.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
        Summary = "Buscar proveedor por ID",
        Description = "Buscar objeto por proveedorID",
        OperationId = "buscar_proveedor_por_id")]
        [ProducesResponseType(typeof(ProveedorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProveedorDTO>> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener proveedor con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar proveedor.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("buscar-por-ruc/{numeroDocumento}")]
        [SwaggerOperation(
        Summary = "Buscar proveedor por ruc ó número de documento",
        Description = "Buscar objeto por numeroDocumento del proveedor",
        OperationId = "buscar_proveedor_por_ruc")]
        [ProducesResponseType(typeof(ProveedorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProveedorDTO>> GetByNumeroDocumento(string numeroDocumento)
        {
            try
            {
                var result = await _service.GetByNumeroDocumentoAsync(numeroDocumento);
                if (result == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener proveedor con el ruc: {numeroDocumento}. Verifique si el proveedor existe en la bbdd."
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar proveedor.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpPost]
        [SwaggerOperation(
        Summary = "Registrar un proveedor",
        Description = "Registra un proveedor en la bbdd",
        OperationId = "registrar_proveedor")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create(ProveedorCreateDTO proveedorDto)
        {
            try
            {
                var newId = await _service.CreateAsync(proveedorDto);
                return StatusCode(201, new CreatedDTO { Id = newId });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al registrar proveedor.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpPut]
        [SwaggerOperation(
        Summary = "Actualizar un proveedor",
        Description = "Actualizar proveedor en la bbdd",
        OperationId = "actualizar_proveedor")]
        [ProducesResponseType(typeof(ProveedorUpdateDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProveedorUpdateDTO>> Update(int id, ProveedorUpdateDTO proveedorDto)
        {
            try
            {
                var updatedProveedor = await _service.UpdateAsync(id, proveedorDto);
                return Ok(updatedProveedor);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar proveedor.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpDelete]
        [SwaggerOperation(
        Summary = "Eliminar un proveedor",
        Description = "Elimina un proveedor de la bbdd",
        OperationId = "eliminar_proveedor")]
        [ProducesResponseType(typeof(DeletedDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new DeletedDTO { Id = id });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al eliminar proveedor.",
                    Detalle = errorMsg
                });
            }
        }
    }
}
