using Genexs.AWS.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente;

namespace MUTZI.GUIA.APPLICATION.Services
{
    public class S3FileService: IS3FileService
    {
        public IConfiguration _configuration;
        private readonly IS3Configuration _appConfiguration;
        private readonly IS3Service _aws3Services;
        public S3FileService(IConfiguration configuration, IS3Configuration appConfiguration)
        {
            _configuration = configuration;
            _appConfiguration = appConfiguration;
            _aws3Services = new S3Service(_appConfiguration.AwsAccessKey,
                _appConfiguration.AwsSecretAccessKey, _appConfiguration.AwsSessionToken,
                _appConfiguration.Region, _configuration["Aws:BucketS3"]);
        }
        public async Task<S3ObjectResponse?> GetFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("El nombre del archivo es requerido");

            try
            {
                S3ObjectResponse? objectResponse = await _aws3Services.DownloadFileAsync(fileName);

                if (objectResponse?.ResponseStream == null)
                    throw new Exception("Archivo no encontrado");

                return objectResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al descargar el archivo: {fileName}");
            }
        }
        public async Task<bool> DeleteFileAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("El nombre del archivo es requerido");

            try
            {
                bool deleted = await _aws3Services.DeleteFileAsync(fileName);

                if (!deleted) { throw new Exception($"No se pudo eliminar el archivo {fileName}"); }

                return deleted;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el archivo: {fileName}");
            }
        }
        public async Task<bool> UploadFileAsync(IFormFile file)
        {
            if (file is null || file.Length <= 0)
                throw new Exception("El archivo es requerido");

            try
            {
                bool uploaded = await _aws3Services.UploadFileAsync(file);

                if (!uploaded) { throw new Exception($"No se pudo guardar el archivo {file.FileName}"); }

                return uploaded;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar el archivo: {file.FileName}");
            }
        }
        public async Task<bool> UploadFileWithCustomNameAsync(IFormFile file, string customName)
        {
            if (file is null || file.Length <= 0)
                throw new Exception("El archivo es requerido");

            try
            {
                bool uploaded = await _aws3Services.UploadFileAsync(file, customName);

                if (!uploaded) { throw new Exception($"No se pudo guardar el archivo {customName}"); }

                return uploaded;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar el archivo: {customName}");
            }
        }
    }
}
