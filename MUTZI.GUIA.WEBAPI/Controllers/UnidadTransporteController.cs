using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Proveedor;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadTransporte;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.WEBAPI.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("unidad-transporte")]
    [ApiController]
    [Authorize]
    public class UnidadTransporteController : ControllerBase
    {
        private readonly IUnidadTransporteService _service;
        public UnidadTransporteController(IUnidadTransporteService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("listar/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Listar Unidad de UnidadTransporte",
            Description = "Lista las unidades de transporte. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
            OperationId = "listar_unidad_transporte"
        )]
        [Permission("listar_unidad_transporte")]
        [ProducesResponseType(typeof(PagedResult<UnidadTransporteDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<UnidadTransporteDTO>>> GetAll(int pageNumber, int pageSize, [FromQuery] string? search)
        {
            try
            {
                var q = Request.Query;
                var result = await _service.GetListAsync(pageNumber, pageSize, search);
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
            OperationId = "listar_unidad_transporte_sugerido"
        )]
        [Permission("listar_unidad_transporte_sugerido")]
        [ProducesResponseType(typeof(UnidadTransporteListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<UnidadTransporteListDTO>>> GetBySuggestion([FromQuery] string? numeroDoc)
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
            OperationId = "buscar_unidad_transporte_por_id"
        )]
        [Permission("buscar_unidad_transporte_por_id")]
        [ProducesResponseType(typeof(UnidadTransporteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UnidadTransporteDTO>> GetById(int id)
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
            OperationId = "registrar_unidad_transporte"
        )]
        [Permission("registrar_unidad_transporte")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create(UnidadTransporteCreateDTO transporteDto)
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
            OperationId = "editar_unidad_transporte"
        )]
        [Permission("editar_unidad_transporte")]
        [ProducesResponseType(typeof(ResponseDTO<UnidadTransporteDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDTO<UnidadTransporteDTO>>> Update(int id, [FromBody] UnidadTransporteUpdateDTO transporteDto)
        {
            try
            {
                var updatedTransporte = await _service.UpdateAsync(id, transporteDto);
                return Ok(new ResponseDTO<UnidadTransporteDTO>
                {
                    Data = updatedTransporte,
                    Detalle = $"Datos de la unidad de transporte COD-{id.ToString("D4")} editados con éxito."
                });
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
            OperationId = "eliminar_unidad_transporte"
        )]
        [Permission("eliminar_unidad_transporte")]
        [ProducesResponseType(typeof(DeletedDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new DeletedDTO { Id = id, Detalle = "Se eliminó la unidad de transporte con éxito." });
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
            OperationId = "editar_estado_unidad_transporte"
        )]
        [Permission("editar_estado_unidad_transporte")]
        [ProducesResponseType(typeof(ResponseDTO<EstadoActualizadoResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDTO<EstadoActualizadoResponseDTO>>> CambiarEstado(int id, [FromBody] EstadoActualizadoRequestDTO infoRequestDto)
        {
            try
            {
                var result = await _service.CambiarEstadoAsync(id, infoRequestDto);
                return Ok(new ResponseDTO<EstadoActualizadoResponseDTO>
                {
                    Data = result,
                    Detalle = $"Se actualizó el estado de la unidad de transporte con código: COD-{id.ToString("D4")} al estado '{result.Estado}'."
                });
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
