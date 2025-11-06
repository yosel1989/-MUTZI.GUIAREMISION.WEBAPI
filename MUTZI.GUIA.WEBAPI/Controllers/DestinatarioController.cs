using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Conductor;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Destinatario;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente.Destinatario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DestinatarioController : ControllerBase
    {
        private readonly IDestinatarioService _destinatarioService;
        public DestinatarioController(IDestinatarioService destinatarioService)
        {
            _destinatarioService = destinatarioService;
        }
        [HttpGet]
        [Route("listar")]
        [SwaggerOperation(
        Summary = "Listar destinatarios",
        Description = "Lista los destinatarios",
        OperationId = "listar_destinatario")]
        [ProducesResponseType(typeof(DestinatarioDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<DestinatarioDTO>>> GetAll()
        {
            try
            {
                var destinatarios = await _destinatarioService.GetAllAsync();
                if (destinatarios == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = "No se pudo obtener la lista de destinatarios."
                    });
                }

                return Ok(destinatarios);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar destinatarios.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
        Summary = "Buscar destinatario por ID",
        Description = "Buscar objeto por destinatarioID",
        OperationId = "buscar_destinatario_por_id")]
        [ProducesResponseType(typeof(DestinatarioDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DestinatarioDTO>> GetById(int id)
        {
            try
            {
                var destinatario = await _destinatarioService.GetByIdAsync(id);
                if (destinatario == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener destinatario con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(destinatario);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar destinatario.",
                    Detalle = errorMsg
                });
            }
            
        }
        [HttpPost]
        [SwaggerOperation(
        Summary = "Registrar un destinatario",
        Description = "Registra un destinatario en la bbdd",
        OperationId = "registrar_destinatario")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create([FromBody] DestinatarioCreateDTO createDTO)
        {
            try
            {
                var createdId = await _destinatarioService.CreateAsync(createDTO);
                return StatusCode(201, new CreatedDTO { Id = createdId });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al registrar destinatario.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpPut]
        [SwaggerOperation(
        Summary = "Actualizar un destinatario",
        Description = "Actualizar destinatario en la bbdd",
        OperationId = "actualizar_destinatario")]
        [ProducesResponseType(typeof(DestinatarioUpdateDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DestinatarioUpdateDTO>> Update(int id, [FromBody] DestinatarioUpdateDTO updateDTO)
        {
            try
            {
                var updated = await _destinatarioService.UpdateAsync(id, updateDTO);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar destinatario.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpDelete]
        [SwaggerOperation(
        Summary = "Eliminar un destinatario",
        Description = "Elimina un destinatario de la bbdd",
        OperationId = "eliminar_destinatario")]
        [ProducesResponseType(typeof(DeletedDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _destinatarioService.DeleteAsync(id);
                return Ok(new DeletedDTO { Id = id });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al eliminar destinatario.",
                    Detalle = errorMsg
                });
            }
        }
    }
}
