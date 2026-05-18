using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Area;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("areas")]
    [ApiController]
 
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _service;
        public AreaController(IAreaService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("listar-para-select")]
        [SwaggerOperation(
            Summary = "Listar areas",
            Description = "Lista los areas activos para seleccionar",
            OperationId = "listar_areas_para_select"
        )]
        [ProducesResponseType(typeof(List<AreaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AreaDTO>>> GetAllToSelect()
        {
            try
            {
                var areas = await _service.ListToSelectAsync();
                return Ok(areas);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar areas.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
