using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Permiso
{
    public class PermisoDTO
    {
        [JsonPropertyName("id")]
        public int PermisoId { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = null!;

        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; } = null!;

        [JsonPropertyName("codigo")]
        public string Codigo { get; set; }

        [JsonPropertyName("id_estado")]
        public int IdEstado { get; set; } = 1;

    }
}
