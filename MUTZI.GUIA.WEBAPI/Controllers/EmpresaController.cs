using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.VentaConfig;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class EmpresaController : ControllerBase
    {
        private readonly IVentaConfigService _service;
        public EmpresaController(IVentaConfigService ventaConfigService)
        {
            _service = ventaConfigService;
        }
        [HttpGet]
        //[Route("")]
        [SwaggerOperation(
        Summary = "Listar empresas para filtro",
        Description = "Lista empresas para select específico de filtro.",
        OperationId = "listar_empresas_nombres")]
        [ProducesResponseType(typeof(EmpresaListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EmpresaListDTO>>> GetOnlyName()
        {
            try
            {
                var empresas = await _service.GetOnlyNameAsync();
                return Ok(empresas);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar empresas.",
                    Detalle = errorMsg
                });
            }
        }
    }
}
