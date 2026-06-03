using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Persona;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("personal")]
    [ApiController]
 
    public class PersonaController : ControllerBase
    {
        private readonly IPersonaService _service;
        public PersonaController(IPersonaService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("listar-sugerida")]
        [SwaggerOperation(
            Summary = "Listar personas sugeridas",
            Description = "Lista las personas sugeridos por nombre, apellido",
            OperationId = "listar_persona_sugerida"
        )]
        [ProducesResponseType(typeof(IEnumerable<PersonaSugeridaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PersonaSugeridaDTO>>> GetBySuggestion([FromQuery] string? text, [FromQuery] string? cargo)
        {
            try
            {
                var Personas = await _service.GetListBySuggestion(text, cargo);
                return Ok(Personas);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar personas.",
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
