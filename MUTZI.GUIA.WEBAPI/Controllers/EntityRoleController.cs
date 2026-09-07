using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.EntityRole;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("entity-roles")]
    [ApiController]
    [Authorize]

    public class EntityRoleController : ControllerBase
    {
        private readonly IEntityRoleService _service;
        public EntityRoleController(IEntityRoleService service)
        {
            _service = service;
        }

        [HttpGet("to-select")]
        [SwaggerOperation(
            Summary = "Listar roles",
            Description = "Consultar roles para seleccionar",
            OperationId = "entity_role_list"
        )]
        [ProducesResponseType(typeof(IEnumerable<EntityRoleToSelectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<EntityRoleToSelectDTO>> ToSelect()
        {
            try
            {
                var list = _service.GetListToSelect();
                return Ok(list);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar roles para seleccionar.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpGet("list-by-entity/{entityId}")]
        [SwaggerOperation(
            Summary = "Listar roles por entidad",
            Description = "Consultar roles de una entidad especifica",
            OperationId = "entity_role_list_by_entity"
        )]
        [ProducesResponseType(typeof(IEnumerable<EntityRoleDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EntityRoleDTO>>> ListByEntity(int entityId)
        {
            try
            {
                var list = await _service.GetByEntityAsync(entityId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar roles de la entidad.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpPut("sync-to-entity/{entityId}")]
        [SwaggerOperation(
            Summary = "Listar roles por entidad",
            Description = "Consultar roles de una entidad especifica",
            OperationId = "entity_role_sync_to_entity"
        )]
        [ProducesResponseType(typeof(IEnumerable<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> SyncToEntityAsync(int entityId, [FromBody] EntityRoleToSyncToEntityDTO request)
        {
            try
            {
                var list = await _service.SyncToEntityAsync(request);
                return Ok(list);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al sincronizar los roles con la entidad.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
