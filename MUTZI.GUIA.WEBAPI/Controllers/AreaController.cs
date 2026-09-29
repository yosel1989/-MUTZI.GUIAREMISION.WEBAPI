using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs;
using MUTZI.GUIA.APPLICATION.DTOs.Area;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.WEBAPI.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("areas")]
    [ApiController]
    [Authorize]
 
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _service;
        public AreaController(IAreaService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("to-select")]
        [SwaggerOperation(
            Summary = "Listar areas",
            Description = "Listar areas activas para seleccionar",
            OperationId = "area_to_select"
        )]
        //[Permission("listas_areas_para_seleccionar")]
        [ProducesResponseType(typeof(List<AreaToSelectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<AreaToSelectDTO>>> GetAllToSelect([FromQuery] int? level) 
        {
            try
            {
                var areas = await _service.GetListToSelectAsync(level);
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
        [Route("to-table/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Listar areas para mostrar en tabla",
            Description = "Lista las areas. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
            OperationId = "area_to_table")
        ]
        //[Permission("listar_areas_para_tabla")]
        [ProducesResponseType(typeof(PagedResult<AreaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<AreaDTO>>> GetAll(int pageNumber, int pageSize, [FromQuery] string? search)
        {
            try
            {
                var q = Request.Query;
                var result = await _service.GetToTableAsync(pageNumber, pageSize, search);
                return Ok(result);
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
        [Route("{areaId}")]
        [SwaggerOperation(
            Summary = "Buscar area por ID",
            Description = "Buscar objeto por areaId",
            OperationId = "area_by_id"
        )]
        //[Permission("buscar_area_por_id")]
        [ProducesResponseType(typeof(AreaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AreaDTO>> GetById(int areaId)
        {
            try
            {
                var result = await _service.GetByIdAsync(areaId);
                if (result == null)
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener el area con el ID: {areaId}. Verifique si el ID existe en la bbdd."
                    });
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar area.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar un area",
            Description = "Registra un area en la bbdd",
            OperationId = "area_create"
        )]
        //[Permission("registrar_area")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create(AreaCreateDTO requeset)
        {
            try
            {
                var newId = await _service.CreateAsync(requeset);
                return StatusCode(201, new CreatedDTO { Id = newId });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al registrar un area.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar un area",
            Description = "Actualiza un area en la bbdd",
            OperationId = "area_update"
        )]
        //[Permission("editar_area")]
        [ProducesResponseType(typeof(ResponseDTO<AreaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ResponseDTO<AreaDTO>>> Update(int id, [FromBody] AreaUpdateDTO request)
        {
            try
            {
                var updated = await _service.UpdateAsync(request);
                return Ok(new ResponseDTO<AreaDTO>
                {
                    Data = updated,
                    Detalle = $"Datos del area COD-{id.ToString("D4")} editados con éxito."
                });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar un area.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar un area",
            Description = "Elimina un area de la bbdd",
            OperationId = "area_delete"
        )]
        //[Permission("eliminar_area")]
        [ProducesResponseType(typeof(DeletedDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new DeletedDTO { Id = id, Detalle = "Se eliminó el conductor con éxito." });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al eliminar conductor.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPut]
        [Route("{id}/toogle-active")]
        [SwaggerOperation(
            Summary = "Actualizar estado de un conductor",
            Description = "Actualizar el estado de un conductor en la bbdd. 1='activo', 0='inactivo'.",
            OperationId = "actualizar_estado_conductor"
        )]
        //[Permission("editar_estado_area")]
        [ProducesResponseType(typeof(ResponseDTO<ToggleActiveResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ToggleActive(int id, [FromBody] bool active)
        {
            try
            {
                var result = await _service.ToggleActiveAsync(id, active);
                return Ok(new ResponseDTO<ToggleActiveResponseDTO>
                {
                    Data = result,
                    Detalle = $"Se {(active ? "activo" : "desactivo")} el area COD-{id.ToString("D4")} con éxito."
                });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = $"Error al {(active ? "activar" : "desactivar")} el area.",
                    Detalle = errorMsg
                });
            }
        }
    }
}
