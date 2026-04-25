using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Establecimiento
{
    public class EstablecimientoDTO
    {
        [JsonPropertyName("id")]
        public int EstablecimientoId { get; set; }

        [JsonPropertyName("ruc")]
        public string Ruc { get; set; } = null!;

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; } = null!;

        [JsonPropertyName("ubigeo_id")]
        public string UbigeoId { get; set; } = null!;

        [JsonPropertyName("departamento")]
        public string Departamento { get; set; } = null!;

        [JsonPropertyName("provincia")]
        public string Provincia { get; set; } = null!;

        [JsonPropertyName("distrito")]
        public string Distrito { get; set; } = null!;

        [JsonPropertyName("direccion")]
        public string Direccion { get; set; } = null!;

        [JsonPropertyName("pais")]
        public string Pais { get; set; } = null!;

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("serie")]
        public string Serie { get; set; } = null!;

        [JsonPropertyName("codigoSunat")]
        public string? CodigoSunat { get; set; }

        [JsonPropertyName("fechaCreacion")]
        public DateTime? FechaCreacion { get; set; }

        [JsonPropertyName("fechaEdicion")]
        public DateTime? FechaEdicion { get; set; }

        [JsonPropertyName("empleadoIDCreacion")]
        public int? EmpleadoIdcreacion { get; set; }

        [JsonPropertyName("empleadoNombreCreacion")]
        public string? EmpleadoNombreCreacion { get; set; }

        [JsonPropertyName("empleadoIDEdicion")]
        public int? EmpleadoIdedicion { get; set; }

        [JsonPropertyName("EmpleadoNombreEdicion")]
        public string? EmpleadoNombreEdicion { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("idEstado")]
        public sbyte? IdEstado { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }
    }
}
