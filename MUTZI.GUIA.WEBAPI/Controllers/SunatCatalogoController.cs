using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.MotivoTraslado;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("sunat-catalogo")]
    [ApiController]
    [Authorize]
    public class MotivoTrasladoController : ControllerBase
    {
        private readonly IMotivoTrasladoService _service;
        public MotivoTrasladoController(IMotivoTrasladoService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("motivos-traslado")]
        [SwaggerOperation(
            Summary = "Listar los motivos de traslado",
            Description = "Lista los motivos de traslados activos para seleccionar",
            OperationId = "listar_motivos_traslado"
        )]
        [ProducesResponseType(typeof(List<MotivoTrasladoToSelectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MotivoTrasladoToSelectDTO>>> GetAllToSelect()
        {
            try
            {
                var motivosTraslado = await _service.ListToSelectAsync();
                return Ok(motivosTraslado);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar los motivos de traslado.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
