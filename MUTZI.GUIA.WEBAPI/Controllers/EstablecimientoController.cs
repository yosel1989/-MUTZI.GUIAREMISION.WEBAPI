using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.DataTable;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.Establecimiento;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente.Remitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("establecimientos")]
    [ApiController]
    [Authorize]
    public class EstablecimientoController : ControllerBase
    {
        private readonly IEstablecimientoService _service;
        public EstablecimientoController(IEstablecimientoService establecimientoService)
        {
            _service = establecimientoService;
        }

        [HttpGet]
        [Route("listar/{pageNumber}/{pageSize}")]
        [SwaggerOperation(
        Summary = "Listar establecimientos",
        Description = "Lista los establecimientos por página. Para ello se debe ingresar como parámetro: el número de página 'pageNumber' y el número máximo de registros dentro de una página 'pageSize'.",
        OperationId = "listar_establecimiento")]
        [ProducesResponseType(typeof(PagedResult<EstablecimientoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResult<EstablecimientoDTO>>> GetAll(int pageNumber, int pageSize, [FromQuery] string? search)
        {
            try
            {
                var remitentes = await _service.GetListAsync(pageNumber, pageSize, search);
                return Ok(remitentes);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar establecimientos.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpGet]
        [Route("listar-select/por-ruc/{ruc}")]
        [SwaggerOperation(
            Summary = "Listar establecimientos para select",
            Description = "Lista los establecimientos para select",
            OperationId = "listar_establecimiento_select")
        ]
        [ProducesResponseType(typeof(EstablecimientoListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstablecimientoListDTO>>> GetToSelect(string ruc)
        {
            try
            {
                var remitentes = await _service.GetAllByRucToSelectAsync(ruc);
                return Ok(remitentes);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar establecimientos.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("listar-sugerido/{ruc}")]
        [SwaggerOperation(
        Summary = "Listar establecimientos sugeridos",
        Description = "Lista los establecimientos sugeridos por número de documento o razón social, de una empresa especifica",
        OperationId = "listar_establecimento_sugerido")]
        [ProducesResponseType(typeof(EstablecimientoListToModalDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstablecimientoListToModalDTO>>> GetBySuggestion(string ruc, [FromQuery] string? search)
        {
            try
            {
                var data = await _service.GetListBySuggestionAsync(ruc, search);
                return Ok(data);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar establecimientos.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
        Summary = "Buscar establecimiento por ID",
        Description = "Buscar objeto por establecimientoID",
        OperationId = "buscar_establecimiento_por_id")]
        [ProducesResponseType(typeof(EstablecimientoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoDTO>> GetById(int id)
        {
            try
            {
                var remitente = await _service.GetByIdAsync(id);
                if (remitente == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener el establecimiento con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(remitente);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar establecimiento.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(
        Summary = "Eliminar establecimiento",
        Description = "Elimina un establecimiento de la bbdd",
        OperationId = "eliminar_establecimiento")]
        [ProducesResponseType(typeof(DeletedDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new DeletedDTO { Id = id });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al eliminar remitente.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPut]
        [Route("{id}/actualizar-estado")]
        [SwaggerOperation(
        Summary = "Actualizar estado de un establecimiento",
        Description = "Actualizar el estado de un establecimiento en la bbdd. 1='activo', 0='inactivo'.",
        OperationId = "actualizar_estado_establecimiento")]
        [ProducesResponseType(typeof(UpdatedStatusDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] InfoRequestDTO infoRequestDto)
        {
            try
            {
                var result = await _service.CambiarEstadoAsync(id, infoRequestDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar el estado de un establecimiento.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPost]
        [SwaggerOperation(
        Summary = "Registrar un establecimiento",
        Description = "Registra un establecimiento en la bbdd",
        OperationId = "registrar_establecimiento")]
        [ProducesResponseType(typeof(CreatedDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> Create([FromBody] EstablecimientoCreateDTO createDTO)
        {
            try
            {
                var createdId = await _service.CreateAsync(createDTO);
                return StatusCode(201, new CreatedDTO { Id = createdId });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al registrar establecimiento.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(
        Summary = "Actualizar un establecimiento",
        Description = "Actualiza establecimiento en la bbdd",
        OperationId = "actualizar_establecimiento")]
        [ProducesResponseType(typeof(EstablecimientoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoDTO>> Update(int id, [FromBody] EstablecimientoUpdateDTO updateDTO)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateDTO);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al actualizar establecimiento.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("buscar-por-id-para-guia/{id}/{tipoGuia}")]
        [SwaggerOperation(
        Summary = "Buscar establecimiento remitente por ID y por tipo de guía",
        Description = "Buscar objeto por ID y tipo de guía. En 'tipoGuia', colocar: 'REMITENTE' ó 'TRANSPORTISTA'.",
        OperationId = "buscar_establecimiento_por_id_guia")]
        [ProducesResponseType(typeof(EstablecimientoRemitenteDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoRemitenteDTO>> GetForGuiaById(int id, string tipoGuia)
        {
            try
            {
                var establecimientoRemitente = await _service.GetByIdForGuiaAsync(id, tipoGuia);
                if (establecimientoRemitente == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener el establecimiento remitente con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(establecimientoRemitente);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar establecimiento remitente.",
                    Detalle = errorMsg
                });
            }
        }


        /*
        [HttpGet]
        [Route("listar-nombres")]
        [SwaggerOperation(
        Summary = "Listar remitentes para filtro",
        Description = "Lista los remitentes para select específico de filtro.",
        OperationId = "listar_remitente_nombres")]
        [ProducesResponseType(typeof(EstablecimientoNameListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstablecimientoNameListDTO>>> GetOnlyName()
        {
            try
            {
                var remitentes = await _service.GetOnlynameAsync();
                return Ok(remitentes);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar remitentes.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("listar-sugerido/{numeroDoc}")]
        [SwaggerOperation(
        Summary = "Listar remitentes sugeridos",
        Description = "Lista los remitentes sugeridos por número de documento o razón social",
        OperationId = "listar_remitente_sugerido")]
        [ProducesResponseType(typeof(EstablecimientoListDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EstablecimientoListDTO>>> GetBySuggestion(string numeroDoc)
        {
            try
            {
                var proveedores = await _service.GetListBySuggestion(numeroDoc);
                return Ok(proveedores);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar remitentes.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("buscar-por-id/{id}")]
        [SwaggerOperation(
        Summary = "Buscar remitente por ID",
        Description = "Buscar objeto por remitenteID",
        OperationId = "buscar_remitente_por_id")]
        [ProducesResponseType(typeof(EstablecimientoDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoDTO>> GetById(int id)
        {
            try
            {
                var remitente = await _service.GetByIdAsync(id);
                if (remitente == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener remitente con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(remitente);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar remitente.",
                    Detalle = errorMsg
                });
            }
        }
        [HttpGet]
        [Route("buscar-por-id-para-guia/{id}/{tipoGuia}")]
        [SwaggerOperation(
        Summary = "Buscar remitente por ID y por tipo de guía",
        Description = "Buscar objeto por ID y tipo de guía. En 'tipoGuia', colocar: 'REMITENTE' ó 'TRANSPORTISTA'.",
        OperationId = "buscar_remitente_por_id_guia")]
        [ProducesResponseType(typeof(EstablecimientoForGuiaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EstablecimientoForGuiaDTO>> GetForGuiaById(int id, string tipoGuia)
        {
            try
            {
                var remitente = await _service.GetByIdForGuiaAsync(id, tipoGuia);
                if (remitente == null)
                {
                    return BadRequest(new BadRequestResponseDTO
                    {
                        Error = "No se encontraron registros",
                        Detalle = $"No se pudo obtener remitente con el ID: {id}. Verifique si el ID existe en la bbdd."
                    });
                }
                return Ok(remitente);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al buscar remitente.",
                    Detalle = errorMsg
                });
            }
        }
        
        
        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(
        Summary = "Eliminar remitente",
        Description = "Elimina un remitente de la bbdd",
        OperationId = "eliminar_remitente")]
        [ProducesResponseType(typeof(DeletedDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok(new DeletedDTO { Id = id });
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al eliminar remitente.",
                    Detalle = errorMsg
                });
            }
        }
        */
    }
}
