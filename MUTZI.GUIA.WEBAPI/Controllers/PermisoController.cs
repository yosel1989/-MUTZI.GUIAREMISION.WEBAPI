using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Permiso;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("permisos")]
    [ApiController]
    [Authorize]
    public class PermisoController : ControllerBase
    {
        private readonly IPermisoService _service;
        public PermisoController(IPermisoService perfilService)
        {
            _service = perfilService;
        }


        [HttpGet]
        [Route("listar")]
        [SwaggerOperation(
            Summary = "Listar permisos",
            Description = "Lista los permisos activos",
            OperationId = "listar_permisos"
        )]
        [ProducesResponseType(typeof(List<PermisoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PermisoDTO>>> GetAll()
        {
            try
            {
                var permisos = await _service.ListAsync();
                return Ok(permisos);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar permisos.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpPost]
        [Route("asignar-perfiles")]
        [SwaggerOperation(
            Summary = "Asignar perfiles",
            Description = "Acción de asignar perfiles a los permisos",
            OperationId = "permiso_asignar_perfiles"
        )]
        //[ProducesResponseType(typeof(List<PermisoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AsignarPerfiles(List<PermisoAsignarPerfilesDTO> request)
        {
            try
            {
                await _service.AsignarPerfiles(request);
                return Ok();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al asignar perfiles.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
