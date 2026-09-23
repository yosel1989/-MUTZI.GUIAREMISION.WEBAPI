using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranchSerie;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.WEBAPI.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("security-personal-entity-branch-series")]
    [ApiController]
    [Authorize]
    public class SecurityPersonEntityBranchSerieController : ControllerBase
    {
        private readonly ISecurityPersonEntityBranchSerieService _service;
        public SecurityPersonEntityBranchSerieController(ISecurityPersonEntityBranchSerieService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{securityPersonId}/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Listar series asignadas a un personal",
            Description = "Lista los series asignadas. Para ello se debe ingresar como parámetro: el id del personal 'securityPersonId',  el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
            OperationId = "listar_establecimiento"
        )]
        [Permission("listar_establecimiento")]
        [ProducesResponseType(typeof(PagedResult<SecurityPersonEntityBranchSerieDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<SecurityPersonEntityBranchSerieDTO>>> GetAll(int securityPersonId, int pageNumber, int pageSize, [FromQuery] string? search)
        {
            try
            {
                var data = await _service.GetListAsync(securityPersonId, pageNumber, pageSize, search);
                return Ok(data);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar las series asignados.",
                    Detalle = errorMsg
                });
            }
        }



        [HttpPost]
        [SwaggerOperation(
            Summary = "Asignar una serie a un personal del sistema",
            Description = "Asigna una serie a un personal del sistema en la BBDD",
            OperationId = "security_person_entity_branch_serie_create"
        )]
        //[Permission("security_person_create")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Create([FromBody] SecurityPersonEntityBranchSerieCreateDTO createDTO)
        {
            try
            {
                var result = await _service.CreateAsync(createDTO);
                return StatusCode(201, result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al asignar la serie al personal del sistema.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpDelete]
        [Route("{securityPersonId}/{entityBranchSerieId}")]
        [SwaggerOperation(
            Summary = "Eliminar una serie asignada",
            Description = "Eliminar una serie asignada de la relación con el personal del sistema.",
            OperationId = "security_person_entity_branch_serie_delete"
        )]
        //[Permission("security_person_entity_branch_delete")]
        [ProducesResponseType(typeof(ActionResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Update(int securityPersonId, int entityBranchSerieId)
        {
            try
            {
                var updated = await _service.DeleteAsync(new SecurityPersonEntityBranchSerieDeleteDTO{ SecurityPersonId = securityPersonId, EntityBranchSerieId = entityBranchSerieId });
                return Ok(updated);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al eliminar una serie de la relación de asignados.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
