using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Establecimiento
{
    public class EntityBranchDTO
    {
        [JsonPropertyName("id")]
        public int EntityBranchId { get; set; }

        [JsonPropertyName("entity_id")]
        public int? EntityId { get; set; }

        [JsonPropertyName("document_number")]
        public string? DocumentNumber { get; set; }

        [JsonPropertyName("ruc")]
        public string Ruc { get; set; } = null!;

        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;

        [JsonPropertyName("ubigeo_id")]
        public string UbigeoId { get; set; } = null!;

        [JsonPropertyName("departamento")]
        public string Departamento { get; set; } = null!;

        [JsonPropertyName("provincia")]
        public string Provincia { get; set; } = null!;

        [JsonPropertyName("distrito")]
        public string Distrito { get; set; } = null!;

        [JsonPropertyName("address")]
        public string Address { get; set; } = null!;

        [JsonPropertyName("pais")]
        public string Pais { get; set; } = null!;

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("serie")]
        public string Serie { get; set; } = null!;

        [JsonPropertyName("code_sunat")]
        public string CodeSunat { get; set; }

        [JsonPropertyName("fecha_registro")]
        public DateTime? FechaRegistro { get; set; }

        [JsonPropertyName("fecha_modifico")]
        public DateTime? FechaModifico { get; set; }

        [JsonPropertyName("usuario_registro")]
        public string UsuarioRegistro { get; set; }

        [JsonPropertyName("usuario_registro_nombre")]
        public string UsuarioRegistroNombre { get; set; }

        [JsonPropertyName("usuario_modifico")]
        public string? UsuarioModifico { get; set; }

        [JsonPropertyName("usuario_modifico_nombre")]
        public string? UsuarioModificoNombre { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("tipo")]
        public string Tipo { get; set; }

        [JsonPropertyName("razon_social")]
        public string RazonSocial { get; set; }

        [JsonPropertyName("area")]
        public string? Area { get; set; }

        [JsonPropertyName("is_main")]
        public bool IsMain { get; set; } = false;
    }
}
