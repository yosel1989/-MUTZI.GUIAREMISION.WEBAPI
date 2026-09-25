using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.SecurityPersonReasonForTransfer;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("security-person-reason-for-transfers")]
    [ApiController]
    [Authorize]
    public class SecurityPersonReasonForTransferController : ControllerBase
    {
        private readonly ISecurityPersonReasonForTransferService _service;
        public SecurityPersonReasonForTransferController(ISecurityPersonReasonForTransferService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{securityPersonId}/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Listar motivos de traslado asignados a un personal",
            Description = "Lista los motivos de traslado. Para ello se debe ingresar como parámetro: el id del personal 'securityPersonId',  el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
            OperationId = "security_personal_reason_for_transfers_by_person"
        )]
        //[Permission("listar_establecimiento")]
        [ProducesResponseType(typeof(PagedResult<SecurityPersonReasonForTransferDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<SecurityPersonReasonForTransferDTO>>> GetAll(int securityPersonId, int pageNumber, int pageSize, [FromQuery] string? search)
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
                    Error = "Error al listar los motivos de traslados asignados.",
                    Detalle = errorMsg
                });
            }
        }



        [HttpPost]
        [SwaggerOperation(
            Summary = "Asignar un motivo de traslado a un personal del sistema",
            Description = "Asigna un motivo de traslado a un personal del sistema en la BBDD",
            OperationId = "security_person_reason_for_transfer_create"
        )]
        //[Permission("security_person_create")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Create([FromBody] SecurityPersonReasonForTransferCreateDTO createDTO)
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
                    Error = "Error al asignar el motivo de traslado al personal del sistema.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpDelete]
        [Route("{securityPersonId}/{ReasonForTransferId}")]
        [SwaggerOperation(
            Summary = "Eliminar un motivo de traslado",
            Description = "Eliminar un motivo de traslado asignado de la relación con el personal del sistema.",
            OperationId = "security_person_reason_for_transfer_delete"
        )]
        //[Permission("security_person_entity_branch_delete")]
        [ProducesResponseType(typeof(ActionResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> Update(int securityPersonId, int ReasonForTransferId)
        {
            try
            {
                var updated = await _service.DeleteAsync(new SecurityPersonReasonForTransferDeleteDTO{ SecurityPersonId = securityPersonId, ReasonForTransferId = ReasonForTransferId });
                return Ok(updated);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al eliminar un motivo de traslado de la relación de asignados.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
