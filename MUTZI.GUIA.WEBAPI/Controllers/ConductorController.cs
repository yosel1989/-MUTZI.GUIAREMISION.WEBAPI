using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Conductor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
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
        [SwaggerOperation(
        Summary = "Listar conductores",
        Description = "Lista los conductores",
        OperationId = "listar_conductor")]
        [ProducesResponseType(typeof(ConductorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ConductorDTO>>> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();
                if (result == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = "No se pudo obtener la lista de conductores."
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar conductores.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
        Summary = "Buscar conductor por ID",
        Description = "Buscar objeto por conductorID",
        OperationId = "buscar_conductor_por_id")]
        [ProducesResponseType(typeof(ConductorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConductorDTO>> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null)
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener conductor con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar conductor.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpPost]
        [SwaggerOperation(
        Summary = "Registrar un conductor",
        Description = "Registra un conductor en la bbdd",
        OperationId = "registrar_conductor")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create(ConductorCreateDTO conductorDto)
        {
            try
            {
                var newId = await _service.CreateAsync(conductorDto);
                return StatusCode(201, new CreatedDTO { Id = newId });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al registrar un conductor.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpPut]
        [SwaggerOperation(
        Summary = "Actualizar un conductor",
        Description = "Actualiza un conductor en la bbdd",
        OperationId = "actualizar_conductor")]
        [ProducesResponseType(typeof(ConductorUpdateDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConductorUpdateDTO>> Update(int id, ConductorUpdateDTO conductorDto)
        {
            try
            {
                var updatedConductor = await _service.UpdateAsync(id, conductorDto);
                return Ok(updatedConductor);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar un conductor.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpDelete]
        [SwaggerOperation(
        Summary = "Eliminar un conductor",
        Description = "Elimina un conductor de la bbdd",
        OperationId = "eliminar_conductor")]
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
                    Error = "Error al eliminar conductor.",
                    Detalle = errorMsg
                });
            }
        }
    }
}
