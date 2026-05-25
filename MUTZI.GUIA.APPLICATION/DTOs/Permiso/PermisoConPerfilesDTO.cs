using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Permiso
{
    public class PermisoConPerfilesDTO
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
        public int EstadoId { get; set; } = 1;

        [JsonPropertyName("perfiles")]
        public List<int> Perfiles { get; set; } = new List<int>();

    }
}
