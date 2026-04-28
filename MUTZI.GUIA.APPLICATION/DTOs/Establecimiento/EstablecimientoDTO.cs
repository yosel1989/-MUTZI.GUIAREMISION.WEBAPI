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

        [JsonPropertyName("codigo_sunat")]
        public string? CodigoSunat { get; set; }

        [JsonPropertyName("fecha_creacion")]
        public DateTime? FechaCreacion { get; set; }

        [JsonPropertyName("fecha_edicion")]
        public DateTime? FechaEdicion { get; set; }

        [JsonPropertyName("empleado_id_creacion")]
        public int? EmpleadoIdcreacion { get; set; }

        [JsonPropertyName("empleado_nombre_creacion")]
        public string? EmpleadoNombreCreacion { get; set; }

        [JsonPropertyName("empleado_id_edicion")]
        public int? EmpleadoIdedicion { get; set; }

        [JsonPropertyName("empleado_nombre_edicion")]
        public string? EmpleadoNombreEdicion { get; set; }

        [JsonPropertyName("estado")]
        public string? Estado { get; set; }

        [JsonPropertyName("id_estado")]
        public sbyte? IdEstado { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("razon_social")]
        public string RazonSocial { get; set; }
    }
}
