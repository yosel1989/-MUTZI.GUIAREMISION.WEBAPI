using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.DTOs.Transportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.WEBAPI.Attributes;
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
            Summary = "Listar Transportista",
            Description = "Lista las empresas de transporte. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
            OperationId = "listar_transportista"
        )]
        [Permission("listar_transportista")]
        [ProducesResponseType(typeof(PagedResult<TransportistaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<TransportistaDTO>>> GetAll(int pageNumber, int pageSize, [FromQuery] string? search)
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
            Summary = "Listar transportistas sugeridos",
            Description = "Lista los transportistas sugeridos por número de documento o razón social",
            OperationId = "listar_transportista_sugerido"
        )]
        [Permission("listar_transportista_sugerido")]
        [ProducesResponseType(typeof(TransportistaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TransportistaDTO>>> GetBySuggestion([FromQuery] string? numeroDoc)
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
            Summary = "Buscar transportista por ID",
            Description = "Buscar un transportista en la bd",
            OperationId = "buscar_transportista_por_id"
        )]
        [Permission("buscar_transportista_por_id")]
        [ProducesResponseType(typeof(TransportistaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransportistaDTO>> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);
                if (result == null) {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se encontró la transportista con ID: {id}. Verifique si el ID existe en la bbdd."
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
            Summary = "Registrar transportista",
            Description = "Registra un transportista",
            OperationId = "registrar_transportista"
        )]
        [Permission("registrar_transportista")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create(TransportistaCreateDTO transporteDto)
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
                    Error = "Error al registrar transportista.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar transportista",
            Description = "Actualiza los datos de un transportista",
            OperationId = "editar_transportista"
        )]
        [Permission("editar_transportista")]
        [ProducesResponseType(typeof(TransportistaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TransportistaDTO>> Update(int id, [FromBody] TransportistaUpdateDTO transporteDto)
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
                    Error = "Error al actualizar transportista.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar transportista",
            Description = "Elimina un transportista de la bbdd",
            OperationId = "eliminar_transportista"
        )]
        [Permission("eliminar_transportista")]
        [ProducesResponseType(typeof(DeletedDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new DeletedDTO { Id = id, Detalle = "Se eliminó el transportista con éxito." });
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
            Summary = "Actualizar estado de un transportista",
            Description = "Actualizar el estado de un transportista en la bbdd. 1='activo', 0='inactivo'.",
            OperationId = "editar_estado_transportista"
        )]
        [Permission("editar_estado_transportista")]
        [ProducesResponseType(typeof(EstadoActualizadoResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] EstadoActualizadoRequestDTO infoRequestDto)
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
                    Error = "Error al actualizar el estado de una transportista.",
                    Detalle = errorMsg
                });
            }
        }
    }
}
