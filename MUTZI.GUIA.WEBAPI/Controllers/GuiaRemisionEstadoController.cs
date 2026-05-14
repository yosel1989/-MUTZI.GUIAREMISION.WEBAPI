using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemisionEstado;
using MUTZI.GUIA.APPLICATION.DTOs.Permiso;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using MUTZI.GUIA.WEBAPI.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("guia-remision-estados")]
    [ApiController]
    [Authorize]
    public class GuiaRemisionEstadoController : ControllerBase
    {
        private readonly IGuiaRemisionEstadoService _service;
        public GuiaRemisionEstadoController(IGuiaRemisionEstadoService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "Listar estados de la guía de remisión",
            Description = "Lista los estados permitidos para una guia de remisión",
            OperationId = "guia_remision_estado_listar"
        )]
        //[Permission("guia_remision_estado_listar")]
        [ProducesResponseType(typeof(List<GuiaRemisionEstadoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GuiaRemisionEstadoDTO>>> GetAll()
        {
            try
            {
                var data = await _service.GetAllAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar estados.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpGet("permisos")]
        [SwaggerOperation(
            Summary = "Listar estados de la guía de remisión con sus permisos",
            Description = "Lista los estados permitidos para una guia de remisión con sus respectivos estados permitidos para proceder al cambio",
            OperationId = "guia_remision_estado_permisos_listar"
        )]
        //[Permission("guia_remision_estado_permisos_listar")]
        [ProducesResponseType(typeof(List<GuiaRemisionEstadoWithPermisoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GuiaRemisionEstadoWithPermisoDTO>>> GetAllWithPermisos()
        {
            try
            {
                var data = await _service.GetAllWithPermisosAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar estados con permisos.",
                    Detalle = errorMsg
                });
            }
        }




        [HttpPost]
        [Route("asignar-permisos")]
        [SwaggerOperation(
            Summary = "Asignar permisos",
            Description = "Acción de asignar permisos a estados para cambiar a un estado especifico",
            OperationId = "permiso_asignar_perfiles"
        )]
        //[ProducesResponseType(typeof(List<PermisoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AsignarPerfiles(List<GuiaRemisionEstadoAsignarPermisosDTO> request)
        {
            try
            {
                await _service.AsignarPermisosAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al asignar permisos.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
