using Genexs.AWS.Library;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTZI.GUIA.APPLICATION.Interfaces.Services.GuiaRemitente
{
    public interface IS3FileService
    {
        Task<S3ObjectResponse?> GetFileAsync(string fileName);

        Task<bool> DeleteFileAsync(string fileName);

        Task<bool> UploadFileAsync(IFormFile file);

        Task<bool> UploadFileWithCustomNameAsync(IFormFile file, string customName);
    }
}
