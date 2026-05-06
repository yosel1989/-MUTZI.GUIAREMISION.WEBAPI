using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.WEBAPI.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("establecimientos")]
    [ApiController]
    [Authorize]
    public class EstablecimientoController : ControllerBase
    {
        private readonly IEstablecimientoService _service;
        public EstablecimientoController(IEstablecimientoService establecimientoService)
        {
            _service = establecimientoService;
        }

        [HttpGet]
        [Route("listar/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Listar establecimientos",
            Description = "Lista los establecimientos por página. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
            OperationId = "listar_establecimiento"
        )]
        [Permission("listar_establecimiento")]
        [ProducesResponseType(typeof(PagedResult<EstablecimientoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<EstablecimientoDTO>>> GetAll(int pageNumber, int pageSize, [FromQuery] string? search)
        {
            try
            {
                var remitentes = await _service.GetListAsync(pageNumber, pageSize, search);
                return Ok(remitentes);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar establecimientos.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpGet]
        [Route("listar-select/por-ruc/{ruc}")]
        [SwaggerOperation(
            Summary = "Listar establecimientos para select",
            Description = "Lista los establecimientos para select",
            OperationId = "listar_establecimiento_select")
        ]
        [Permission("listar_establecimiento_select")]
        [ProducesResponseType(typeof(EstablecimientoListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstablecimientoListDTO>>> GetToSelect(string ruc)
        {
            try
            {
                var remitentes = await _service.GetAllByRucToSelectAsync(ruc);
                return Ok(remitentes);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar establecimientos.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("listar-sugerido/{ruc}")]
        [SwaggerOperation(
            Summary = "Listar establecimientos sugeridos",
            Description = "Lista los establecimientos sugeridos por número de documento o razón social, de una empresa especifica",
            OperationId = "listar_establecimento_sugerido"
        )]
        [Permission("listar_establecimiento_sugerido")]
        [ProducesResponseType(typeof(EstablecimientoListToModalDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstablecimientoListToModalDTO>>> GetBySuggestion(string ruc, [FromQuery] string? search)
        {
            try
            {
                var data = await _service.GetListBySuggestionAsync(ruc, search);
                return Ok(data);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar establecimientos.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
            Summary = "Buscar establecimiento por ID",
            Description = "Buscar objeto por establecimientoID",
            OperationId = "buscar_establecimiento_por_id"
        )]
        [Permission("buscar_establecimiento_por_id")]
        [ProducesResponseType(typeof(EstablecimientoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoDTO>> GetById(int id)
        {
            try
            {
                var remitente = await _service.GetByIdAsync(id);
                if (remitente == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener el establecimiento con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(remitente);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar establecimiento.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar establecimiento",
            Description = "Elimina un establecimiento de la bbdd",
            OperationId = "eliminar_establecimiento"
        )]
        [Permission("eliminar_establecimiento")]
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
                    Error = "Error al eliminar remitente.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPut]
        [Route("{id}/actualizar-estado")]
        [SwaggerOperation(
            Summary = "Actualizar estado de un establecimiento",
            Description = "Actualizar el estado de un establecimiento en la bbdd. 1='activo', 0='inactivo'.",
            OperationId = "editar_estado_establecimiento"
        )]
        [Permission("editar_estado_establecimiento")]
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
                    Error = "Error al actualizar el estado de un establecimiento.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar un establecimiento",
            Description = "Registra un establecimiento en la bbdd",
            OperationId = "registrar_establecimiento"
        )]
        [Permission("registrar_establecimiento")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create([FromBody] EstablecimientoCreateDTO createDTO)
        {
            try
            {
                var createdId = await _service.CreateAsync(createDTO);
                return StatusCode(201, new CreatedDTO { Id = createdId });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al registrar establecimiento.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar un establecimiento",
            Description = "Actualiza establecimiento en la bbdd",
            OperationId = "editar_establecimiento"
        )]
        [Permission("editar_establecimiento")]
        [ProducesResponseType(typeof(EstablecimientoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoDTO>> Update(int id, [FromBody] EstablecimientoUpdateDTO updateDTO)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateDTO);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar establecimiento.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("buscar-por-id-para-guia/{id}/{tipoGuia}")]
        [SwaggerOperation(
            Summary = "Buscar establecimiento remitente por ID y por tipo de guía",
            Description = "Buscar objeto por ID y tipo de guía. En 'tipoGuia', colocar: 'REMITENTE' ó 'TRANSPORTISTA'.",
            OperationId = "buscar_establecimiento_por_id_guia"
        )]
        [Permission("buscar_establecimiento_por_id_guia")]
        [ProducesResponseType(typeof(EstablecimientoRemitenteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoRemitenteDTO>> GetForGuiaById(int id, string tipoGuia)
        {
            try
            {
                var establecimientoRemitente = await _service.GetByIdForGuiaAsync(id, tipoGuia);
                if (establecimientoRemitente == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener el establecimiento remitente con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(establecimientoRemitente);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar establecimiento remitente.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
