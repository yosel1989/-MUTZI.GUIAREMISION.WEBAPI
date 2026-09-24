using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.EntityBranchSerie;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("entity-branch-series")]
    [ApiController]
    [Authorize]
    public class EntityBranchSerieController : ControllerBase
    {
        private readonly IEntityBranchSerieService _service;
        public EntityBranchSerieController(IEntityBranchSerieService serieService)
        {
            _service = serieService;
        }

        [HttpGet]
        [Route("branch/{entityBranchId}/list/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Listar series",
            Description = "Lista las series por página. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
            OperationId = "entity_branch_serie_list"
        )]
        //[Permission("entity_branch_serie_list")]
        [ProducesResponseType(typeof(PagedResult<EntityBranchSerieDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<EntityBranchSerieDTO>>> GetAll(int entityBranchId, int pageNumber, int pageSize, [FromQuery] string? search)
        {
            try
            {
                var remitentes = await _service.GetListAsync(entityBranchId, pageNumber, pageSize, search);
                return Ok(remitentes);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar series.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("branch/{entityBranchId}/to-select")]
        [SwaggerOperation(
            Summary = "Listar series para seleccionar",
            Description = "Lista las series para seleccionar por entidad. Para ello se debe ingresar como parámetro: entityBranchId",
            OperationId = "entity_branch_serie_to_select"
        )]
        //[Permission("entity_branch_serie_list")]
        [ProducesResponseType(typeof(List<EntityBranchSerieToSelectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EntityBranchSerieToSelectDTO>>> GetAllToSelect(int entityBranchId, [FromQuery] string? search)
        {
            try
            {
                var remitentes = await _service.GetAllByEntityToSelectModal(entityBranchId, search);
                return Ok(remitentes);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar series.",
                    Detalle = errorMsg
                });
            }
        }



        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Buscar serie por ID",
            Description = "Buscar objeto por serieID",
            OperationId = "entity_branch_serie_by_id"
        )]
        //[Permission("entity_branch_serie_by_id")]
        [ProducesResponseType(typeof(EntityBranchSerieDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EntityBranchSerieDTO>> GetById(int id)
        {
            try
            {
                var remitente = await _service.GetByIdAsync(id);
                if (remitente == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener la serie con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(remitente);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar serie.",
                    Detalle = errorMsg
                });
            }
        }

        /*[HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar serie",
            Description = "Elimina un serie de la bbdd",
            OperationId = "eliminar_serie"
        )]
        [Permission("eliminar_serie")]
        [ProducesResponseType(typeof(DeletedDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new DeletedDTO { Id = id, Detalle = "Se eliminó el serie con éxito." });
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
        }*/

        [HttpPut]
        [Route("{id}/toggle-active")]
        [SwaggerOperation(
            Summary = "Actualizar estado de una serie",
            Description = "Actualizar el estado de una serie en la bbdd. 1='activo', 0='inactivo'.",
            OperationId = "entity_branch_serie_toggle_active"
        )]
        //[Permission("entity_branch_serie_toggle_active")]
        [ProducesResponseType(typeof(ResponseDTO<ToggleActiveResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ToggleActive(int id, [FromBody] EstadoActualizadoRequestDTO infoRequestDto)
        {
            try
            {
                var result = await _service.ToggleActiveAsync(id, infoRequestDto.Active);
                return Ok(new ResponseDTO<ToggleActiveResponseDTO>
                {
                    Data = result,
                    Detalle = $"Se {(result.Active ? "activó" : "desactivó")} la serie COD-{id.ToString("D4")} con éxito."
                });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar el estado de un serie.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar una serie",
            Description = "Registra una serie en la bbdd",
            OperationId = "entity_branch_serie_create"
        )]
        //[Permission("entity_branch_serie_create")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create([FromBody] EntityBranchSerieCreateDTO createDTO)
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
                    Error = "Error al registrar serie.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar un serie",
            Description = "Actualiza serie en la bbdd",
            OperationId = "entity_branch_serie_update"
        )]
        //[Permission("entity_branch_serie_update")]
        [ProducesResponseType(typeof(EntityBranchSerieDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EntityBranchSerieDTO>> Update(int id, [FromBody] EntityBranchSerieUpdateDTO request)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, request);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar serie.",
                    Detalle = errorMsg
                });
            }
        }


    }
}
