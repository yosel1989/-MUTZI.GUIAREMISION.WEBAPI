using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionHistorial;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("guia-remision-historial")]
    [ApiController]
    [Authorize]
    public class GuiaRemisionHistorialController : ControllerBase
    {
        private readonly IGuiaRemisionHistorialService _service;
        public GuiaRemisionHistorialController(IGuiaRemisionHistorialService service)
        {
            _service = service;
        }

        [HttpGet("{guiaRemisionId}")]
        [SwaggerOperation(
            Summary = "Listar el historial registrado de una guía de remisión",
            Description = "Listar el historial registrado de una guía de remisión",
            OperationId = "listar_guia_remision_historial"
        )]
        [ProducesResponseType(typeof(IEnumerable<GuiaRemisionHistorialListDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GuiaRemisionHistorialListDTO>>> GetHistorialByGuia(int guiaRemisionId)
        {
            try
            {
                var result = await _service.GetAllByGuiaAsync(guiaRemisionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar historial.",
                    Detalle = errorMsg
                });
            }
        }
    }
}
