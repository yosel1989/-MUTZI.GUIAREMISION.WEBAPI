using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Persona;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;
using MUTZI.GUIA.APPLICATION.DTOs.Entity;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("entities")]
    [ApiController]
 
    public class EntityController : ControllerBase
    {
        private readonly IEntityService _service;
        public EntityController(IEntityService service)
        {
            _service = service;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar una entidad",
            Description = "Registrar una entidad con su tipo y roles",
            OperationId = "entity_create"
        )]
        [ProducesResponseType(typeof(IEnumerable<EntityDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EntityDTO>>> Create([FromBody] EntityCreateDTO request)
        {
            try
            {
                var newEntity = await _service.CreateAsync(request);
                return StatusCode(StatusCodes.Status201Created, newEntity);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al registrar entidad.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Buscar personal por id",
            Description = "Buscar la información de un personal por su id",
            OperationId = "buscar_personal_por_id"
        )]
        [ProducesResponseType(typeof(PersonaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonaDTO>> GetById(int id)
        {
            try
            {
                var personal = await _service.GetByIdAsync(id);
                return Ok(personal);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar al personal",
                    Detalle = errorMsg
                });
            }
        }

    }
}
