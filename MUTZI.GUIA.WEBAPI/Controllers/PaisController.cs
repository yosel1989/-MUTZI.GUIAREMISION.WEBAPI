using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Pais;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Pais;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PaisController : ControllerBase
    {
        private readonly IPaisService _service;
        public PaisController(IPaisService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("paises")]
        [SwaggerOperation(
        Summary = "Listar países definidos",
        Description = "Lista países definidos para mostrarlos dentro de un select.",
        OperationId = "listar_pais")]
        [ProducesResponseType(typeof(PaisesDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<PaisesDTO>> GetPaises()
        {
            try
            {
                var result = _service.GetPaisesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar países definidos.",
                    Detalle = errorMsg
                });
            }
        }
    }
}
