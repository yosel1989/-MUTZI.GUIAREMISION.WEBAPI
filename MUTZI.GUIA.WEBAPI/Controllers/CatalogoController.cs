using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Catalogos;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("catalogos")]
    [ApiController]
    [Authorize]
    public class CatalogoController : ControllerBase
    {
        private readonly ICatalogoService _service;
        public CatalogoController(ICatalogoService service)
        {
            _service = service;
        }
        [HttpGet]
        [Route("paises")]
        [SwaggerOperation(
        Summary = "Listar países definidos",
        Description = "Lista países definidos para mostrarlos dentro de un select.",
        OperationId = "listar_pais")]
        [ProducesResponseType(typeof(PaisDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<PaisDTO>> GetPaises()
        {
            try
            {
                var result = _service.GetPaises();
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

        [HttpGet]
        [Route("emisor-vehicular")]
        [SwaggerOperation(
        Summary = "Listar emisoras de autorización vehicular",
        Description = "Lista entidades emisoras de la autorización vehícular.",
        OperationId = "listar_emisora_vehicular")]
        [ProducesResponseType(typeof(EmisorAutorizadorVehicularDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<EmisorAutorizadorVehicularDTO>> GetEmisorVehicular()
        {
            try
            {
                var result = _service.GetEmisoresVehiculares();
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar emisores vehiculares.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpGet]
        [Route("tipo-establecimiento")]
        [SwaggerOperation(
        Summary = "Listar tipos de establecimiento",
        Description = "Lista tipos de establecimiento según SUNAT.",
        OperationId = "listar_tipo_establecimiento")]
        [ProducesResponseType(typeof(IEnumerable<TipoEstablecimientoDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<TipoEstablecimientoDTO>> GetTipoEstablecimientoLista()
        {
            try
            {
                var result = _service.GetTiposEstablecimiento();
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar tipos de establecimiento.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("tipos-documento")]
        [SwaggerOperation(
            Summary = "Listar tipos de documento",
            Description = "Lista tipos de documento para seleccionar",
            OperationId = "listar_tipo_documento"
        )]
        [ProducesResponseType(typeof(IEnumerable<TipoDocumentoToSelectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TipoDocumentoToSelectDTO>>> GetTipoDocumentoLista([FromQuery] string? tipoRegimen)
        {
            try
            {
                var result = await _service.GetTiposDocumento(tipoRegimen);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar tipos de documento del catalogo.",
                    Detalle = errorMsg
                });
            }
        }

        [HttpGet]
        [Route("entidades-reguladoras")]
        [SwaggerOperation(
            Summary = "Listar entidades reguladoras",
            Description = "Lista todas las entidades reguladoras",
            OperationId = "listar_entidad_reguladora"
        )]
        [ProducesResponseType(typeof(IEnumerable<TipoDocumentoToSelectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TipoDocumentoToSelectDTO>>> GetEntidadesReguladoras()
        {
            try
            {
                var result = await _service.GetEntidadesReguladoras();
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar entidades reguladoras del catalogo.",
                    Detalle = errorMsg
                });
            }
        }


        [HttpGet]
        [Route("unidades-medida")]
        [SwaggerOperation(
            Summary = "Listar unidades de medida",
            Description = "Lista todas las unidades de medida",
            OperationId = "listar_unidad_medida"
        )]
        [ProducesResponseType(typeof(IEnumerable<UnidadMedidaToSelectDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResponseDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(InternalErrorResponseDTO), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<UnidadMedidaToSelectDTO>>> GetUnidadesMedida([FromQuery] string? tipo)
        {
            try
            {
                var result = await _service.GetUnidadesMedida(tipo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new InternalErrorResponseDTO
                {
                    Error = "Error al listar las unidades de medida del catalogo.",
                    Detalle = errorMsg
                });
            }
        }
    }
}
