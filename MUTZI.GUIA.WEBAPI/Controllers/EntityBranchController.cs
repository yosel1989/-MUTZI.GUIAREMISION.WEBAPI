using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.WEBAPI.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("entity-branchs")]
    [ApiController]
    [Authorize]
    public class EntityBranchController : ControllerBase
    {
        private readonly IEntityBranchService _service;
        public EntityBranchController(IEntityBranchService establecimientoService)
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
        [ProducesResponseType(typeof(PagedResult<EntityBranchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<EntityBranchDTO>>> GetAll(int pageNumber, int pageSize, [FromQuery] string? search)
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
        [Route("list-by-entity/{entityId}")]
        [SwaggerOperation(
            Summary = "Listar establecimientos",
            Description = "Lista los establecimientos por entidad, para seleccionar",
            OperationId = "listar_establecimiento_select")
        ]
        [Permission("listar_establecimiento_select")]
        [ProducesResponseType(typeof(List<EntityBranchToSelectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EntityBranchToSelectDTO>>> GetToSelect(int entityId)
        {
            try
            {
                var remitentes = await _service.GetAllByEntityToSelect2Async(entityId);
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
        [Route("listar-sugerido/{entityId}")]
        [SwaggerOperation(
            Summary = "Listar establecimientos sugeridos",
            Description = "Lista los establecimientos sugeridos por número de documento o razón social, de una empresa especifica",
            OperationId = "listar_establecimento_sugerido"
        )]
        [Permission("listar_establecimiento_sugerido")]
        [ProducesResponseType(typeof(EntityBranchListToModalDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EntityBranchListToModalDTO>>> GetBySuggestion(int entityId, [FromQuery] string? search)
        {
            try
            {
                var data = await _service.GetListBySuggestionAsync(entityId, search);
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
        [ProducesResponseType(typeof(EntityBranchDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EntityBranchDTO>> GetById(int id)
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
                return Ok(new DeletedDTO { Id = id, Detalle = "Se eliminó el establecimiento con éxito." });
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
        [Route("{id}/toggle-active")]
        [SwaggerOperation(
            Summary = "Actualizar estado de un establecimiento",
            Description = "Actualizar el estado de un establecimiento en la bbdd. 1='activo', 0='inactivo'.",
            OperationId = "editar_estado_establecimiento"
        )]
        [Permission("editar_estado_establecimiento")]
        [ProducesResponseType(typeof(ResponseDTO<ToggleActiveResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ToggleActive(int id, [FromBody] EstadoActualizadoRequestDTO infoRequestDto)
        {
            try
            {
                var result = await _service.ToggleActiveAsync(id, infoRequestDto);
                return Ok(new ResponseDTO<ToggleActiveResponseDTO>
                {
                    Data = result,
                    Detalle = $"Se {(result.Active ? "activó" : "desactivó")} el establecimiento COD-{id.ToString("D4")} con éxito."
                });
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
        public async Task<ActionResult<int>> Create([FromBody] EntityBranchCreateDTO createDTO)
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
        [ProducesResponseType(typeof(EntityBranchDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EntityBranchDTO>> Update(int id, [FromBody] EntityBranchUpdateDTO updateDTO)
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
        [Route("buscar-por-id-para-guia/{id}/{tipoGuia}/{entityBranchSerieId}")]
        [SwaggerOperation(
            Summary = "Buscar establecimiento remitente por ID y por tipo de guía",
            Description = "Buscar objeto por ID y tipo de guía. En 'tipoGuia', colocar: 'REMITENTE' ó 'TRANSPORTISTA'.",
            OperationId = "obtener_serie_numero_establecimiento"
        )]
        [Permission("obtener_serie_numero_establecimiento")]
        [ProducesResponseType(typeof(EstablecimientoRemitenteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoRemitenteDTO>> GetForGuiaById(int id, string tipoGuia, int entityBranchSerieId)
        {
            try
            {
                var establecimientoRemitente = await _service.GetByIdForGuiaAsync(id, tipoGuia, entityBranchSerieId);
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
