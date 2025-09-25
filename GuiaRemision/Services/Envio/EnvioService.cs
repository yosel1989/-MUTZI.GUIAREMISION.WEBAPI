using GuiaRemision.DTOs.Efact;
using GuiaRemision.DTOs.GuiaRemitente;
using GuiaRemision.DTOs.GuiaTransportista;
using GuiaRemision.Services.GuiaRemision;
using GuiaRemision.Services.GuiaTransportista;
using GuiaRemision.Services.VentaConfig;
using System.Net.Http.Headers;
using System.Text.Json;

namespace GuiaRemision.Services.Envio
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
        public EnvioService(IConfiguration config, IGuiaRemitenteService guiaRemitenteBDService, IGuiaTransportistaService guiaTransportistaBDService, IGuiaRemitenteUBLService guiaRemitenteUBLService, IGuiaTransportistaUBLService guiaTransportistaUBLService, IVentaConfigService ventaConfigService, HttpClient httpClient)
        {
            _config = config;
            _guiaRemitenteBDService = guiaRemitenteBDService;
            _guiaTransportistaBDService = guiaTransportistaBDService;
            _guiaRemitenteUBLService = guiaRemitenteUBLService;
            _guiaTransportistaUBLService = guiaTransportistaUBLService;
            _ventaConfigService = ventaConfigService;
            _httpClient = httpClient;
            Url_Base_Efact = config["AppSettings:url_base_facturador"];
        }

        public async Task<string> EnviarGuiaElectronicaAsync(string ruc, string nombreArchivoJson)
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
            var url = $"{this.Url_Base_Efact}/api-efact-ose/v1/document";
            var response = await _httpClient.PostAsync(url, form);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error al enviar a EFACT: {response.StatusCode} - {result}");

            return result;
        }

        public void EscribirLog(string tipoGuia, string ruc, string numeroGuia, string estado, string ticket, string detalle)
        {
            try
            {
                // Leer el path base desde la configuración
                string pathLogBase = _config["AppSettings:path_log"];

                // Validación por si está vacío o no se configuró
                if (string.IsNullOrWhiteSpace(pathLogBase))
                {
                    Console.WriteLine("La ruta del log (path_log) no está configurada.");
                    return;
                }

                // Crear subcarpeta por tipo de guía (REMITENTE o TRANSPORTISTA)
                string carpetaTipo = Path.Combine(pathLogBase, tipoGuia);
                if (!Directory.Exists(carpetaTipo))
                {
                    Directory.CreateDirectory(carpetaTipo);
                }

                // Elegir archivo de log según estado
                string archivoLog = estado == "ENVIADO" ? "Success.txt" : "LogError.txt";
                string pathArchivo = Path.Combine(carpetaTipo, archivoLog);

                // Armar contenido del log
                string contenido = $"Fecha: {DateTime.Now:yyyy-MM-dd HH:mm:ss}; RUC: {ruc}; Número Guía: {numeroGuia}; Ticket: {ticket}\nEstado: {estado}\nDetalle: {detalle}\n--------------------------\n";

                // Escribir en el archivo
                File.AppendAllText(pathArchivo, contenido);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al escribir el log: {ex.Message}");
            }
        }

        public async Task<object> ProcesarGuiaRemisionAsync(string ruc, GuiaRemitenteCreateDTO doc)
        {
            string ticket = "";
            string estado = "";

            try
            {
                var (esValido, error) = _guiaRemitenteBDService.ValidarDatosGuiaRemitente(doc);
                if (!esValido)
                {
                    return new BadRequestResponseDto
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

                    return new BadRequestResponseDto
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

                    EscribirLog("GUIAREMITENTE", ruc, guiaCreada.SerieCorrelativo, estado, ticket, errorMsg);
                    return new InternalErrorResponseDto
                    {
                        Error = "Error al enviar a EFACT",
                        Detalle = errorMsg
                    };
                }

                EfactResponseDto efactParsed;
                try
                {
                    var json = JsonDocument.Parse(efactRawResponse).RootElement;
                    efactParsed = new EfactResponseDto
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
                    efactParsed = new EfactResponseDto
                    {
                        Code = "-1",
                        Description = ticket
                    };
                }

                await _guiaRemitenteBDService.UpdateRespuestaTicketAsync(guiaCreada.GuiaID, ticket, estado);
                EscribirLog("GUIAREMITENTE", ruc, guiaCreada.SerieCorrelativo, estado, ticket, efactRawResponse);

                return new EnviarGuiaResponseDto
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

                EscribirLog("GUIAREMITENTE", ruc, "NumeroDocRef:" + doc.NumeroDocRef, estado, ticket, errorMsg);

                return new InternalErrorResponseDto
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
                var (esValido, error) = _guiaTransportistaBDService.ValidarDatosGuiaTransportista(doc);
                if (!esValido)
                {
                    return new BadRequestResponseDto
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
                    return new BadRequestResponseDto
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
                    EscribirLog("GUIATRANSPORTISTA", ruc, guiaCreada.SerieCorrelativo, estado, ticket, errorMsg);
                    return new InternalErrorResponseDto
                    {
                        Error = "Error al enviar a EFACT",
                        Detalle = errorMsg
                    };
                }
                // Extraer el ticket
                EfactResponseDto efactParsed;
                try
                {
                    //Si todo está correcto , efactRawResponse es un JSON con "code" y "description"
                    var json = JsonDocument.Parse(efactRawResponse).RootElement;
                    efactParsed = new EfactResponseDto
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
                    efactParsed = new EfactResponseDto
                    {
                        Code = "-1",
                        Description = ticket
                    };
                }

                await _guiaRemitenteBDService.UpdateRespuestaTicketAsync(guiaCreada.GuiaID, ticket, estado);
                EscribirLog("GUIATRANSPORTISTA", ruc, guiaCreada.SerieCorrelativo, estado, ticket, efactRawResponse);

                return new EnviarGuiaResponseDto
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
                EscribirLog("GUIATRANSPORTISTA", ruc, "NumeroDocRef:" + doc.NumeroDocRef, estado, ticket, errorMsg);
                return new InternalErrorResponseDto
                {
                    Error = "Error inesperado",
                    Detalle = errorMsg
                };
            }
        }
    }
}
