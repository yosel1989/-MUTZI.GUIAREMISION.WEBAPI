using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.IntegrationCredential;
using MUTZI.GUIA.APPLICATION.DTOs.Shared;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("integration-credentials")]
    [ApiController]
    [Authorize]

    public class IntegrationCredentialController : ControllerBase
    {
        private readonly IIntegrationCredentialService _service;
        public IntegrationCredentialController(IIntegrationCredentialService service)
        {
            _service = service;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar una credencial de integración",
            Description = "Registrar una credencial de integración credencial de integración y sus apiKey",
            OperationId = "integration_credential_create"
        )]
        [ProducesResponseType(typeof(IEnumerable<IntegrationCredentialDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<IntegrationCredentialDTO>>> Create([FromBody] IntegrationCredentialCreateDTO request)
        {
            try
            {
                var newIntegrationCredential = await _service.CreateAsync(request);
                return StatusCode(StatusCodes.Status201Created, newIntegrationCredential);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al registrar la credencial de integración.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Actualizar una credencial de integración",
            Description = "Actualizar una credencial de integración con su apiKey",
            OperationId = "integration_credential_update"
        )]
        [ProducesResponseType(typeof(IEnumerable<IntegrationCredentialDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<IntegrationCredentialDTO>>> Update(int id, [FromBody] IntegrationCredentialUpdateDTO request)
        {
            try
            {
                var newIntegrationCredential = await _service.UpdateAsync(id, request);
                return Ok(newIntegrationCredential);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al modificar credencial de integración.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(
            Summary = "Buscar credencial de integración",
            Description = "Buscar la credencial de integración por su id",
            OperationId = "integration_credential_by_id"
        )]
        [ProducesResponseType(typeof(IntegrationCredentialDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IntegrationCredentialDTO>> GetById(int id)
        {
            try
            {
                var IntegrationCredentiall = await _service.GetByIdAsync(id);
                return Ok(IntegrationCredentiall);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar la credencial de integración.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("collection/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Obtener credenciales de integración",
            Description = "Listar credenciales de integración. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
            OperationId = "integration_credential_collection"
        )]
        [ProducesResponseType(typeof(PagedResult<IntegrationCredentialDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<IntegrationCredentialDTO>>> GetAll(int pageNumber, int pageSize, [FromQuery] string? search)
        {
            try
            {
                var q = Request.Query;
                var result = await _service.GetDataTableAsync(pageNumber, pageSize, search);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar credenciales de integración.",
                    Detalle = errorMsg
                });
            }
        }


        /*[HttpGet]
        [Route("list/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
            Summary = "Listar credencial de integraciónes",
            Description = "Listar credencial de integraciónes. Para ello se debe ingresar como parámetro: el número de página 'pageNumber'.",
            OperationId = "integration_credential_list"
        )]
        [ProducesResponseType(typeof(PagedResult<IntegrationCredentialListDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<IntegrationCredentialListDTO>>> GetList( int pageNumber, int pageSize, [FromQuery] string? search, [FromQuery] string? type)
        {
            try
            {
                var q = Request.Query;
                var result = await _service.GetListAsync(pageNumber, pageSize, search, type);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar proveedores.",
                    Detalle = errorMsg
                });
            }
        }*/


        [HttpPut]
        [Route("{id}/toggle-active")]
        [SwaggerOperation(
            Summary = "Cambiar el estado activo",
            Description = "Activar o desactivar el estado de una credencial de integración",
            OperationId = "integration_credential_toggle_active"
        )]
        [ProducesResponseType(typeof(ResponseDTO<EstadoActualizadoResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] EstadoActualizadoRequestDTO infoRequestDto)
        {
            try
            {
                var result = await _service.ToggleActiveAsync(id, infoRequestDto);
                return Ok(new ResponseDTO<EstadoActualizadoResponseDTO>
                {
                    Data = result,
                    Detalle = $"Se actualizó el estado de la de credencial con código: COD-{id.ToString("D4")} al estado {result.Estado}."
                });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar el estado de la credencial.",
                    Detalle = errorMsg
                });
            }
        }

    }
}
