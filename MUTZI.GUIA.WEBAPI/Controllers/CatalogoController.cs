using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MUTZI.GUIA.APPLICATION.DTOs.Catalogos;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Catalogos;
using Swashbuckle.AspNetCore.Annotations;

namespace MUTZI.GUIA.WEBAPI.Controllers
{
    [Route("[controller]")]
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
    }
}
