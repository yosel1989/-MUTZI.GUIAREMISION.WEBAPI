using System.Text.Json.Serialization;

namespace MUTZI.GUIA.APPLICATION.DTOs.Permiso
{
    public class PermisoAsignarPerfilesDTO
    {
        [JsonPropertyName("id")]
        public int PermisoId { get; set; }

        [JsonPropertyName("perfiles")]
        public List<int> Perfiles { get; set; }
    }
}
