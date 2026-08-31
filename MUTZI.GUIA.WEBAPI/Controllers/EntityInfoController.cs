using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.PersonaEmpresaInfo;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("entity-info")]
    [ApiController]
 
    public class EntityInfoController : ControllerBase
    {
        private readonly IEntityInfoService _service;
        public EntityInfoController(IEntityInfoService service)
        {
            _service = service;
        }

        [HttpGet("dni/{document_number}")]
        [SwaggerOperation(
            Summary = "Buscar información de una persona",
            Description = "Buscar información de una persona por número de documento",
            OperationId = "entity_info_person"
        )]
        [ProducesResponseType(typeof(IEnumerable<PersonInfoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PersonInfoDTO>>> GetEntityInfoPerson(string document_number)
        {
            try
            {
                var entityInfo = await _service.GetPersonInfoAsync(document_number);
                return Ok(entityInfo);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al consultar información de la persona.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet("ruc/{document_number}")]
        [SwaggerOperation(
            Summary = "Buscar información de una empresa",
            Description = "Buscar información de una empresa por número de documento",
            OperationId = "entity_info_company"
        )]
        [ProducesResponseType(typeof(IEnumerable<CompanyInfoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CompanyInfoDTO>>> GetEntityInfoCompany(string document_number)
        {
            try
            {
                var entityInfo = await _service.GetCompanyInfoAsync(document_number);
                return Ok(entityInfo);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al consultar información de la empresa.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
