using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista.RemiteTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Transportista;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("transportista")]
    [ApiController]
    [Authorize]
    public class TransportistaController : ControllerBase
    {
        private readonly ITransportistaService _service;
        public TransportistaController(ITransportistaService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("listar/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
        Summary = "Listar Unidad de Transporte",
        Description = "Lista las unidades de transporte. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
        OperationId = "listar_transporte")]
        [ProducesResponseType(typeof(PagedResult<RemiteTransportistaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<RemiteTransportistaDTO>>> GetAll(int pageNumber, int pageSize)
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
                    Error = "Error al listar unidades de transporte.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("listar-sugerido")]
        [SwaggerOperation(
        Summary = "Listar unidades de transportes sugeridos",
        Description = "Lista las unidades de transporte sugeridos por número de documento o razón social",
        OperationId = "listar_transporte_sugerido")]
        [ProducesResponseType(typeof(RemiteTransportistaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RemiteTransportistaDTO>>> GetBySuggestion([FromQuery] string? numeroDoc)
        {
            try
            {
                var transporte = await _service.GetListBySuggestion(numeroDoc);
                return Ok(transporte);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar unidades de transporte.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
        Summary = "Buscar unidad de transporte por ID",
        Description = "Buscar objeto por transporteID",
        OperationId = "buscar_transporte_por_id")]
        [ProducesResponseType(typeof(RemiteTransportistaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RemiteTransportistaDTO>> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null) {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se encontró la unidad de transporte con ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar transporte.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpPost]
        [SwaggerOperation(
        Summary = "Registrar unidad de transporte",
        Description = "Registra una unidad de transporte en la bbdd",
        OperationId = "registrar_transporte")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create(RemiteTransportistaCreateDTO transporteDto)
        {
            try
            {
                var newId = await _service.CreateAsync(transporteDto);
                return StatusCode(201, new CreatedDTO { Id = newId });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al registrar unidad de transporte.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
        Summary = "Actualizar unidad de transporte",
        Description = "Actualiza la tabla transporte en la bbdd",
        OperationId = "actualizar_transporte")]
        [ProducesResponseType(typeof(RemiteTransportistaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RemiteTransportistaDTO>> Update(int id, [FromBody] RemiteTransportistaUpdateDTO transporteDto)
        {
            try
            {
                var updatedTransporte = await _service.UpdateAsync(id, transporteDto);
                return Ok(updatedTransporte);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar unidad de transporte.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(
        Summary = "Eliminar unidad de transporte",
        Description = "Elimina una unidad de transporte de la bbdd",
        OperationId = "eliminar_transporte")]
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
                    Error = "Error al eliminar transporte.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpPut]
        [Route("{id}/actualizar-estado")]
        [SwaggerOperation(
        Summary = "Actualizar estado de una unidad de transporte",
        Description = "Actualizar el estado de una unidad de transporte en la bbdd. 1='activo', 0='inactivo'.",
        OperationId = "actualizar_estado_transporte")]
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
                    Error = "Error al actualizar el estado de una unidad de transporte.",
                    Detalle = errorMsg
                });
            }
        }
    }
}
