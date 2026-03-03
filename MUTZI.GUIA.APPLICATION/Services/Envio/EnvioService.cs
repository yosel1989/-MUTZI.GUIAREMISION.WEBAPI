using Microsoft.Extensions.Configuration;
using MUTZI.GUIA.APPLICATION.DTOs.Efact;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.Envio;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaTransportista;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.VentaConfig;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MUTZI.GUIA.APPLICATION.Services.Envio
{
    public class EnvioService : IEnvioService
    {
        private readonly IConfiguration _config;
        private readonly IGuiaRemitenteService _guiaRemitenteBDService;
        private readonly IGuiaTransportistaService _guiaTransportistaBDService;
        private readonly IGuiaRemitenteUBLService _guiaRemitenteUBLService;
        private readonly IGuiaTransportistaUBLService _guiaTransportistaUBLService;
        private readonly IVentaConfigService _ventaConfigService;
        private readonly HttpClient _httpClient;
        private string Url_Base_Efact = "";
        private readonly IS3FileService _s3FileService;
        private readonly string _baseUrl = "";
        public EnvioService(IConfiguration config, IGuiaRemitenteService guiaRemitenteBDService, IGuiaTransportistaService guiaTransportistaBDService, IGuiaRemitenteUBLService guiaRemitenteUBLService, IGuiaTransportistaUBLService guiaTransportistaUBLService, IVentaConfigService ventaConfigService, HttpClient httpClient, IS3FileService s3FileService)
        {
            _config = config;
            _guiaRemitenteBDService = guiaRemitenteBDService;
            _guiaTransportistaBDService = guiaTransportistaBDService;
            _guiaRemitenteUBLService = guiaRemitenteUBLService;
            _guiaTransportistaUBLService = guiaTransportistaUBLService;
            _ventaConfigService = ventaConfigService;
            _httpClient = httpClient;
            Url_Base_Efact = config["AppSettings:url_base_facturador"];
            _s3FileService = s3FileService;
            _baseUrl = _config["Aws:BaseUrl"] ?? "";
        }

        public async Task<string> EnviarGuiaElectronicaAsync2(string ruc, string nombreArchivoJson)
        {
            var token = await _ventaConfigService.ObtenerTokenAsync(ruc);

            if (token == null || string.IsNullOrEmpty(token.AccessToken))
                throw new InvalidOperationException("No se pudo obtener un token válido de EFACT.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.AccessToken);
            //Armar ruta del archivo en "JSON"
            var rutaPendiente = Path.Combine(_config["AppSettings:path_pendiente"], nombreArchivoJson);
            if (!File.Exists(rutaPendiente))
                throw new FileNotFoundException($"No se encontró el archivo JSON en {rutaPendiente}");
            using var form = new MultipartFormDataContent();
            await using var fileStream = File.OpenRead(rutaPendiente);
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            //Archivo file .json
            form.Add(streamContent, "file", Path.GetFileName(rutaPendiente));

            //POST a EFACT
            var url = $"{Url_Base_Efact}/api-efact-ose/v1/document";
            var response = await _httpClient.PostAsync(url, form);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al enviar a EFACT: {response.StatusCode} - {result}");

            return result;
        }
        public async Task<string> EnviarGuiaElectronicaAsync(string ruc, string nombreArchivoJson)
        {
            var token = await _ventaConfigService.ObtenerTokenAsync(ruc);

            if (token == null || string.IsNullOrEmpty(token.AccessToken))
                throw new InvalidOperationException("No se pudo obtener un token válido de EFACT.");

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.AccessToken);

            // Obtener el archivo JSON desde S3/AWS
            var s3Object = await _s3FileService.GetFileAsync(nombreArchivoJson);
            if (s3Object?.ResponseStream == null)
                throw new FileNotFoundException($"No se encontró el archivo JSON en S3: {nombreArchivoJson}");

            var respObj = (object)s3Object.ResponseStream;
            Stream contentStream;
            if (respObj is Stream s)
            {
                contentStream = s;
                if (contentStream.CanSeek)
                    contentStream.Position = 0;
            }
            else if (respObj is byte[] bytes)
            {
                contentStream = new MemoryStream(bytes);
            }
            else
            {
                throw new Exception("Tipo de ResponseStream no soportado al descargar desde S3.");
            }

            using var form = new MultipartFormDataContent();
            using var streamContent = new StreamContent(contentStream);
            var contentType = string.IsNullOrWhiteSpace(s3Object.ContentType) ? "application/json" : s3Object.ContentType;
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

            // Archivo .json
            form.Add(streamContent, "file", nombreArchivoJson);

            // POST a EFACT
            var url = $"{Url_Base_Efact}/api-efact-ose/v1/document";
            var response = await _httpClient.PostAsync(url, form);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al enviar a EFACT: {response.StatusCode} - {result}");

            return result;
        }

        //public void EscribirLog(string tipoGuia, string ruc, string numeroGuia, string estado, string ticket, string detalle)
        //{
        //    try
        //    {
        //        // Leer el path base desde la configuración
        //        string pathLogBase = _config["AppSettings:path_log"];

        //        // Validación por si está vacío o no se configuró
        //        if (string.IsNullOrWhiteSpace(pathLogBase))
        //        {
        //            Console.WriteLine("La ruta del log (path_log) no está configurada.");
        //            return;
        //        }

        //        // Crear subcarpeta por tipo de guía (REMITENTE o TRANSPORTISTA)
        //        string carpetaTipo = Path.Combine(pathLogBase, tipoGuia);
        //        if (!Directory.Exists(carpetaTipo))
        //        {
        //            Directory.CreateDirectory(carpetaTipo);
        //        }

        //        // Elegir archivo de log según estado
        //        string archivoLog = estado == "ENVIADO" ? "Success.txt" : "LogError.txt";
        //        string pathArchivo = Path.Combine(carpetaTipo, archivoLog);

        //        // Armar contenido del log
        //        string contenido = $"Fecha: {DateTime.Now:yyyy-MM-dd HH:mm:ss}; RUC: {ruc}; Número Guía: {numeroGuia}; Ticket: {ticket}\nEstado: {estado}\nDetalle: {detalle}\n--------------------------\n";

        //        // Escribir en el archivo
        //        File.AppendAllText(pathArchivo, contenido);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error al escribir el log: {ex.Message}");
        //    }
        //}

        public async Task<object> ProcesarGuiaRemisionAsync(string ruc, GuiaRemitenteCreateDTO doc)
        {
            string ticket = "";
            string estado = "";

            try
            {
                var (esValido, error) = _guiaRemitenteBDService.ValidarDatosGuiaRemitente(doc, ruc);
                if (!esValido)
                {
                    return new BadRequestResponseDTO
                    {
                        Error = "Error en datos enviados",
                        Detalle = error
                    };
                }

                GuiaRemitenteDTO guiaCreada;
                try
                {
                    guiaCreada = await _guiaRemitenteBDService.CreateAsync(doc);
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.InnerException?.Message ?? ex.Message;
                    if (errorMsg.Length > 500) errorMsg = errorMsg[..500];

                    string errorBox = errorMsg.Contains("FOREIGN KEY") ? "Error de clave foránea."
                                    : errorMsg.Contains("UnidadMedida") ? "Unidad de medida no válida."
                                    : errorMsg.Contains("String") ? "Error de longitud de campo."
                                    : "";

                    return new BadRequestResponseDTO
                    {
                        Error = "Error al guardar la guía",
                        Detalle = $"{errorBox} Más detalle: {errorMsg}"
                    };
                }
                //Generar JSON
                var nombreArchivo = await _guiaRemitenteUBLService.GenerarGuiaRemisionJson(guiaCreada);
                //Enviar a EFACT
                string efactRawResponse;
                try
                {
                    efactRawResponse = await EnviarGuiaElectronicaAsync(ruc, nombreArchivo);
                }
                catch (Exception ex)
                {
                    estado = "RECHAZADO";
                    string errorMsg = ex.InnerException?.Message ?? ex.Message;
                    if (errorMsg.Length > 500) errorMsg = errorMsg[..500];

                    //EscribirLog("GUIAREMITENTE", ruc, guiaCreada.SerieCorrelativo, estado, ticket, errorMsg);
                    return new InternalErrorResponseDTO
                    {
                        Error = "Error al enviar a EFACT",
                        Detalle = errorMsg
                    };
                }

                EfactResponseDTO efactParsed;
                try
                {
                    var json = JsonDocument.Parse(efactRawResponse).RootElement;
                    efactParsed = new EfactResponseDTO
                    {
                        Code = json.GetProperty("code").GetString() ?? "",
                        Description = json.GetProperty("description").GetString() ?? ""
                    };

                    ticket = efactParsed.Description;
                    estado = "ENVIADO";
                }
                catch
                {
                    ticket = efactRawResponse.Length > 500 ? efactRawResponse[..500] : efactRawResponse;
                    estado = "RECHAZADO";
                    efactParsed = new EfactResponseDTO
                    {
                        Code = "-1",
                        Description = ticket
                    };
                }

                await _guiaRemitenteBDService.UpdateRespuestaTicketAsync(guiaCreada.GuiaID, ticket, estado);
                //EscribirLog("GUIAREMITENTE", ruc, guiaCreada.SerieCorrelativo, estado, ticket, efactRawResponse);

                return new SendGuiaResponseDTO
                {
                    Success = true,
                    EfactResponse = efactParsed
                };
            }
            catch (Exception ex)
            {
                estado = "RECHAZADO";
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                if (errorMsg.Length > 500) errorMsg = errorMsg[..500];

                //EscribirLog("GUIAREMITENTE", ruc, "Remitente Ruc:" + doc.Remitente.Ruc, estado, ticket, errorMsg);

                return new InternalErrorResponseDTO
                {
                    Error = "Error inesperado",
                    Detalle = errorMsg
                };
            }
        }
        public async Task<object> ReenviarGuiaRemisionAsync(string ruc, int guiaId)
        {
            try
            {
                var guiaExistente = await _guiaRemitenteBDService.GetByIdAsync(guiaId);
                if (guiaExistente == null)
                {
                    return new BadRequestResponseDTO
                    {
                        Error = "Guía no encontrada",
                        Detalle = $"No se encontró una guía de remisión con ID {guiaId}."
                    };
                }
                var nombreArchivo = await _guiaRemitenteUBLService.GenerarGuiaRemisionJson(guiaExistente);
                string efactRawResponse;
                try
                {
                    efactRawResponse = await EnviarGuiaElectronicaAsync(ruc, nombreArchivo);
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.InnerException?.Message ?? ex.Message;
                    if (errorMsg.Length > 500) errorMsg = errorMsg[..500];
                    return new InternalErrorResponseDTO
                    {
                        Error = "Error al enviar a EFACT",
                        Detalle = errorMsg
                    };
                }
                EfactResponseDTO efactParsed;
                try
                {
                    var json = JsonDocument.Parse(efactRawResponse).RootElement;
                    efactParsed = new EfactResponseDTO
                    {
                        Code = json.GetProperty("code").GetString() ?? "",
                        Description = json.GetProperty("description").GetString() ?? ""
                    };
                }
                catch
                {
                    string ticket = efactRawResponse.Length > 500 ? efactRawResponse[..500] : efactRawResponse;
                    efactParsed = new EfactResponseDTO
                    {
                        Code = "-1",
                        Description = ticket
                    };
                }
                return new SendGuiaResponseDTO
                {
                    Success = true,
                    EfactResponse = efactParsed
                };
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                if (errorMsg.Length > 500) errorMsg = errorMsg[..500];
                return new InternalErrorResponseDTO
                {
                    Error = "Error inesperado",
                    Detalle = errorMsg
                };
            }
        }

        public async Task<object> ProcesarGuiaTransportistaAsync(string ruc, GuiaTransportistaCreateDTO doc)
        {
            string ticket = "";
            string estado = "";
            try
            {
                var (esValido, error) = _guiaTransportistaBDService.ValidarDatosGuiaTransportista(doc, ruc);
                if (!esValido)
                {
                    return new BadRequestResponseDTO
                    {
                        Error = "Error en datos enviados",
                        Detalle = error
                    };
                }
                // Crear la guía en la BBDD
                GuiaTransportistaDTO guiaCreada;
                try
                {
                    guiaCreada = await _guiaTransportistaBDService.CreateAsync(doc);
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.InnerException?.Message ?? ex.Message;
                    if (errorMsg.Length > 500) errorMsg = errorMsg[..500];
                    string errorBox = errorMsg.Contains("FOREIGN KEY") ? "Error de clave foránea."
                                    : errorMsg.Contains("UnidadMedida") ? "Unidad de medida no válida."
                                    : errorMsg.Contains("String") ? "Error de longitud de campo."
                                    : "";
                    return new BadRequestResponseDTO
                    {
                        Error = "Error al guardar la guía transportista",
                        Detalle = $"{errorBox} Más detalle: {errorMsg}"
                    };
                }
                // Generar JSON
                var nombreArchivo = await _guiaTransportistaUBLService.GenerarGuiaTransportistaJson(guiaCreada);

                // Enviar JSON a EFACT
                string efactRawResponse;
                try
                {
                    efactRawResponse = await EnviarGuiaElectronicaAsync(ruc, nombreArchivo);
                }
                catch (Exception ex)
                {
                    estado = "RECHAZADO";
                    string errorMsg = ex.InnerException?.Message ?? ex.Message;
                    if (errorMsg.Length > 500)
                    {
                        errorMsg = errorMsg[..500];
                    }
                    //EscribirLog("GUIATRANSPORTISTA", ruc, guiaCreada.SerieCorrelativo, estado, ticket, errorMsg);
                    return new InternalErrorResponseDTO
                    {
                        Error = "Error al enviar a EFACT",
                        Detalle = errorMsg
                    };
                }
                // Extraer el ticket
                EfactResponseDTO efactParsed;
                try
                {
                    //Si todo está correcto , efactRawResponse es un JSON con "code" y "description"
                    var json = JsonDocument.Parse(efactRawResponse).RootElement;
                    efactParsed = new EfactResponseDTO
                    {
                        Code = json.GetProperty("code").GetString() ?? "",
                        Description = json.GetProperty("description").GetString() ?? ""
                    };

                    ticket = efactParsed.Description;
                    estado = "ENVIADO";
                }
                catch
                {
                    ticket = efactRawResponse.Length > 500 ? efactRawResponse[..500] : efactRawResponse;
                    estado = "RECHAZADO";
                    efactParsed = new EfactResponseDTO
                    {
                        Code = "-1",
                        Description = ticket
                    };
                }

                await _guiaRemitenteBDService.UpdateRespuestaTicketAsync(guiaCreada.GuiaID, ticket, estado);
                //EscribirLog("GUIATRANSPORTISTA", ruc, guiaCreada.SerieCorrelativo, estado, ticket, efactRawResponse);

                return new SendGuiaResponseDTO
                {
                    Success = true,
                    EfactResponse = efactParsed
                };

            }
            catch (Exception ex)
            {
                estado = "RECHAZADO";
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                if (errorMsg.Length > 500) errorMsg = errorMsg[..500];
                //EscribirLog("GUIATRANSPORTISTA", ruc, "NumeroDocRef:" + doc.NumeroDocRef, estado, ticket, errorMsg);
                return new InternalErrorResponseDTO
                {
                    Error = "Error inesperado",
                    Detalle = errorMsg
                };
            }
        }

        public async Task<object> ReenviarGuiaTransportistaAsync(string ruc, int guiaId)
        {
            try
            {
                var guiaExistente = await _guiaTransportistaBDService.GetByIdAsync(guiaId);
                if (guiaExistente == null)
                {
                    return new BadRequestResponseDTO
                    {
                        Error = "Guía no encontrada",
                        Detalle = $"No se encontró una guía de transportista con ID {guiaId}."
                    };
                }
                var nombreArchivo = await _guiaTransportistaUBLService.GenerarGuiaTransportistaJson(guiaExistente);
                string efactRawResponse;
                try
                {
                    efactRawResponse = await EnviarGuiaElectronicaAsync(ruc, nombreArchivo);
                }
                catch (Exception ex)
                {
                    string errorMsg = ex.InnerException?.Message ?? ex.Message;
                    if (errorMsg.Length > 500) errorMsg = errorMsg[..500];
                    return new InternalErrorResponseDTO
                    {
                        Error = "Error al enviar a EFACT",
                        Detalle = errorMsg
                    };
                }
                EfactResponseDTO efactParsed;
                try
                {
                    var json = JsonDocument.Parse(efactRawResponse).RootElement;
                    efactParsed = new EfactResponseDTO
                    {
                        Code = json.GetProperty("code").GetString() ?? "",
                        Description = json.GetProperty("description").GetString() ?? ""
                    };
                }
                catch
                {
                    string ticket = efactRawResponse.Length > 500 ? efactRawResponse[..500] : efactRawResponse;
                    efactParsed = new EfactResponseDTO
                    {
                        Code = "-1",
                        Description = ticket
                    };
                }
                return new SendGuiaResponseDTO
                {
                    Success = true,
                    EfactResponse = efactParsed
                };
            }
            catch (Exception ex)
            {
                string errorMsg = ex.InnerException?.Message ?? ex.Message;
                if (errorMsg.Length > 500) errorMsg = errorMsg[..500];
                return new InternalErrorResponseDTO
                {
                    Error = "Error inesperado",
                    Detalle = errorMsg
                };
            }

        }
    }
}
