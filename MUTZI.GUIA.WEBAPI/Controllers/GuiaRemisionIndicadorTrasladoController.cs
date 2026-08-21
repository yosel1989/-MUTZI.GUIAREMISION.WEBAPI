using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionEstado;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("guia-remision-indicadores-traslado")]
    [ApiController]
    [Authorize]
    public class GuiaRemisionIndicadorTrasladoController : ControllerBase
    {
        private readonly IGuiaRemisionIndicadorTrasladoService _service;
        public GuiaRemisionIndicadorTrasladoController(IGuiaRemisionIndicadorTrasladoService service)
        {
            _service = service;
        }

        [HttpGet("to-select")]
        [SwaggerOperation(
            Summary = "Listar indicadores de traslado",
            Description = "Lista los indicadores de traslado para guía de remisión",
            OperationId = "indicadores_traslado_to_select"
        )]
        //[Permission("guia_remision_estado_listar")]
        [ProducesResponseType(typeof(List<GuiaRemisionIndicadorTrasladoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GuiaRemisionIndicadorTrasladoDTO>>> GetToSelect()
        {
            try
            {
                var data = await _service.GetToSelectAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar indicadores.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
