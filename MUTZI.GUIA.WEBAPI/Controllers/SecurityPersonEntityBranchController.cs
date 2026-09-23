using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonEntityBranch;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.WEBAPI.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("security-personal-entity-branchs")]
    [ApiController]
    [Authorize]
    public class SecurityPersonEntityBranchController : ControllerBase
    {
        private readonly ISecurityPersonEntityBranchService _service;
        public SecurityPersonEntityBranchController(ISecurityPersonEntityBranchService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{securityPersonId}/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Listar personales de seguridad",
            Description = "Lista los personales de seguridad. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
            OperationId = "listar_establecimiento"
        )]
        [Permission("listar_establecimiento")]
        [ProducesResponseType(typeof(PagedResult<SecurityPersonEntityBranchDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<SecurityPersonEntityBranchDTO>>> GetAll(int securityPersonId, int pageNumber, int pageSize, [FromQuery] string? search)
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
                    Error = "Error al listar los establecimientos asignados.",
                    Detalle = errorMsg
                });
            }
        }



        [HttpPost]
        [SwaggerOperation(
            Summary = "Asignar un establecimiento a un personal del sistema",
            Description = "Registra un establecimiento a un personal del sistema en la BBDD",
            OperationId = "security_person_entity_branch_create"
        )]
        //[Permission("security_person_create")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Create([FromBody] SecurityPersonEntityBranchCreateDTO createDTO)
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
                    Error = "Error al asignar el establecimiento al personal del sistema.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpDelete]
        [Route("{securityPersonId}/{entityBranchId}")]
        [SwaggerOperation(
            Summary = "Eliminar un establecimiento asignado",
            Description = "Eliminar un establecimiento asignado de la relación con el personal del sistema.",
            OperationId = "security_person_entity_branch_delete"
        )]
        //[Permission("security_person_entity_branch_delete")]
        [ProducesResponseType(typeof(ActionResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Update(int securityPersonId, int entityBranchId)
        {
            try
            {
                var updated = await _service.DeleteAsync(new SecurityPersonEntityBranchDeleteDTO{ SecurityPersonId = securityPersonId, EntityBranchId = entityBranchId });
                return Ok(updated);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al eliminar un establecimiento de la relación de asignados.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
