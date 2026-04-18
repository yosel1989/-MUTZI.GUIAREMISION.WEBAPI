using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Conductor;
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
                var q = Request.Query;
                var result = await _service.GetListAsync(pageNumber, pageSize, q);
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
        [Route("listar-sugerido")]
        [SwaggerOperation(
        Summary = "Listar conductores sugeridos",
        Description = "Lista los conductores sugeridos por número de documento o razón social",
        OperationId = "listar_conductor_sugerido")]
        [ProducesResponseType(typeof(ConductorListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ConductorListDTO>>> GetBySuggestion([FromQuery] string? numeroDoc)
        {
            try
            {
                var conductores = await _service.GetListBySuggestion(numeroDoc);
                return Ok(conductores);
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
        [Route("{id}")]
        [SwaggerOperation(
        Summary = "Actualizar un conductor",
        Description = "Actualiza un conductor en la bbdd",
        OperationId = "actualizar_conductor")]
        [ProducesResponseType(typeof(ConductorDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConductorDTO>> Update(int id, [FromBody] ConductorUpdateDTO conductorDto)
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
        [Route("{id}")]
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
        [HttpPut]
        [Route("{id}/actualizar-estado")]
        [SwaggerOperation(
        Summary = "Actualizar estado de un conductor",
        Description = "Actualizar el estado de un conductor en la bbdd. 1='activo', 0='inactivo'.",
        OperationId = "actualizar_estado_conductor")]
        [ProducesResponseType(typeof(UpdatedStatusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] InfoRequestDTO infoRequestDto)
        {
            try
            {
                var result = await _service.CambiarEstadoAsync(id, infoRequestDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar el estado de un conductor.",
                    Detalle = errorMsg
                });
            }
        }
    }
}
