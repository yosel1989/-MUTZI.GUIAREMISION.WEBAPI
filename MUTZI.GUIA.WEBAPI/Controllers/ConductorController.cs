using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Conductor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConductorController : ControllerBase
    {
        private readonly IConductorService _service;
        public ConductorController(IConductorService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("listar/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
        Summary = "Listar conductores",
        Description = "Lista los conductores. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
        OperationId = "listar_conductor")]
        [ProducesResponseType(typeof(PagedResult<ConductorDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<ConductorDTO>>> GetAll(int pageNumber, int pageSize)
        {
            try
            {
                var result = await _service.GetListAsync(pageNumber, pageSize);
                if (result == null || result.Data.Count==0)
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
        [HttpGet]
        [Route("buscar-por-numero-documento/{numeroDocumento}")]
        [SwaggerOperation(
        Summary = "Buscar conductor por Nro documento",
        Description = "Buscar objeto por número de documento.",
        OperationId = "buscar_conductor_por_numero_documento")]
        [ProducesResponseType(typeof(ConductorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConductorDTO>> GetByNumeroDocumento(string numeroDocumento)
        {
            try
            {
                var result = await _service.GetByNumeroDocumentoAsync(numeroDocumento);
                if (result == null)
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener conductor con el Nro Documento: {numeroDocumento}. Verifique si el Nro Documento existe en la bbdd."
                    });
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar conductor por Nro documento.",
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
        [ProducesResponseType(typeof(ConductorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConductorDTO>> Update(int id, ConductorUpdateDTO conductorDto)
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
