using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MUTZI.GUIA.APPLICATION.Helpers
{
    public class Helper
    {
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static bool GuardarJSON(string json, string ruta)
        {
            RemoveJsonFile(ruta);
            File.WriteAllText(ruta, json);
            return true;

        }
        public static bool RemoveJsonFile(string ruta)
        {
            if (File.Exists(ruta))
                File.Delete(ruta);
            return true;
        }
    }
    //public class NumeroDocRefFormatoAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        var dto = (DocRelacionadoCreateDTO)validationContext.ObjectInstance;
    //        var numeroDocRef = value as string;
    //        var tipoDocRef = dto.TipoDocRef?.ToUpper();

    //        if (string.IsNullOrEmpty(numeroDocRef) || string.IsNullOrEmpty(tipoDocRef))
    //            return new ValidationResult("El tipo y número de documento no pueden estar vacíos.");

    //        string facturaPattern = @"^(F[A-Z0-9]{3}|E[0-9]{3}|[0-9]{1,4})-[0-9]{1,8}$";
    //        string boletaPattern = @"^(B[A-Z0-9]{3}|EB[0-9]{2}|[0-9]{1,4})-[0-9]{1,8}$";

    //        switch (tipoDocRef)
    //        {
    //            case "FACTURA":
    //                if (!System.Text.RegularExpressions.Regex.IsMatch(numeroDocRef, facturaPattern))
    //                    return new ValidationResult("El formato del número de factura es inválido. Ejemplo válido: F001-12345");
    //                break;
    //            case "BOLETA":
    //                if (!System.Text.RegularExpressions.Regex.IsMatch(numeroDocRef, boletaPattern))
    //                    return new ValidationResult("El formato del número de boleta es inválido. Ejemplo válido: B001-12345");
    //                break;
    //            default:
    //                return new ValidationResult("Solo se aceptan documentos de tipo FACTURA o BOLETA.");
    //        }

    //        return ValidationResult.Success;
    //    }
    //}
    // Attribute de validación que funciona por reflexión (soporta múltiples DTOs con propiedad TipoDocRef)
    public class NumeroDocRefFormatoAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
                return new ValidationResult("Contexto de validación inválido.");

            var instance = validationContext.ObjectInstance;
            if (instance == null)
                return new ValidationResult("Objeto de validación inválido.");

            // Obtener propiedad 'TipoDocRef' por reflexión para ser compatible con varios DTOs
            var tipoProp = instance.GetType().GetProperty("TipoDocumentoReferencia");
            var tipoDocRef = tipoProp?.GetValue(instance) as string;
            var numeroDocRef = value as string;

            if (string.IsNullOrEmpty(numeroDocRef) || string.IsNullOrEmpty(tipoDocRef))
                return new ValidationResult("El tipo y número de documento no pueden estar vacíos.");

            // Según la documentación, se permiten las siguientes alternativas:
            // Para FACTURA (código 01):
            //  - [F][A-Z0-9]{3}-[0-9]{1,8}       (ej. F001-12345)
            //  - (E001)-[0-9]{1,8}              (ej. E001-12345) -> E[0-9]{3}
            //  - [0-9]{1,4}-[0-9]{1,8}          (ej. 1-12345 o 1234-12345)
            //
            // Para BOLETA (código 03):
            //  - [B][A-Z0-9]{3}-[0-9]{1,8}      (ej. B001-12345)
            //  - (EB01)-[0-9]{1,8}              (ej. EB01-12345) -> EB[0-9]{2}
            //  - [0-9]{1,4}-[0-9]{1,8}
            //
            // Regex combinados que cubren las alternativas:
            string facturaPattern = @"^(?:(?:F[A-Z0-9]{3})|(?:E[0-9]{3})|(?:[0-9]{1,4}))-[0-9]{1,8}$";
            string boletaPattern = @"^(?:(?:B[A-Z0-9]{3})|(?:EB[0-9]{2})|(?:[0-9]{1,4}))-[0-9]{1,8}$";

            switch (tipoDocRef.ToUpperInvariant())
            {
                case "FACTURA":
                    if (!Regex.IsMatch(numeroDocRef, facturaPattern, RegexOptions.IgnoreCase))
                        return new ValidationResult("Formato inválido para FACTURA. Formatos válidos: Fxxx-########, E###-########, o ####-######## (ej. F001-12345, E001-12345, 1-12345).");
                    break;
                case "BOLETA":
                    if (!Regex.IsMatch(numeroDocRef, boletaPattern, RegexOptions.IgnoreCase))
                        return new ValidationResult("Formato inválido para BOLETA. Formatos válidos: Bxxx-########, EB##-########, o ####-######## (ej. B001-12345, EB01-12345, 1-12345).");
                    break;
                default:
                    return new ValidationResult("Solo se aceptan documentos de tipo FACTURA o BOLETA.");
            }

            return ValidationResult.Success;
        }
    }
}
