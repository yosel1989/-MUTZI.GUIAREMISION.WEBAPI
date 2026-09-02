using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Area;
using MUTZI.GUIA.APPLICATION.DTOs.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.WEBAPI.Attributes;
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

        [HttpGet]
        [Route("listar-sugerida")]
        [SwaggerOperation(
            Summary = "Listar areas sugeridas",
            Description = "Lista las areas sugeridos por número de Description",
            OperationId = "listar_area_sugerida"
        )]
        //[Permission("listar_area_sugerida")]
        [ProducesResponseType(typeof(IEnumerable<AreaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AreaDTO>>> GetBySuggestion([FromQuery] string? text)
        {
            try
            {
                var areas = await _service.GetListBySuggestion(text);
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
