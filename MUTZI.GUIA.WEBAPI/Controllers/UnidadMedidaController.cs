using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.UnidadMedida;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("unidad-medida")]
    [ApiController]
    [Authorize]
    public class UnidadMedidaController : ControllerBase
    {
        private readonly IUnidadMedidaService _service;
        public UnidadMedidaController(IUnidadMedidaService perfilService)
        {
            _service = perfilService;
        }

        [HttpGet]
        [Route("listar-para-select")]
        [SwaggerOperation(
            Summary = "Listar unidades de medida",
            Description = "Lista las unidades de medida activos para seleccionar",
            OperationId = "listar_unidad_medida_para_select"
        )]
        [ProducesResponseType(typeof(List<UnidadMedidaToSelectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<UnidadMedidaToSelectDTO>>> GetAllToSelect()
        {
            try
            {
                var data = await _service.ListToSelectAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar unidades de medida.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
