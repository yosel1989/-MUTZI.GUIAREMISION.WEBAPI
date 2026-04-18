using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities;

[Table("unidadTransporte")]
public class UnidadTransporte
{
    public int TransporteId { get; set; }

    public string? Descripcion { get; set; }

    public string? Modelo { get; set; }

    public string? Marca { get; set; }

    public string Placa { get; set; } = null!;

    public string? Tarjeta { get; set; }

    public string? CodEmisorVehicular { get; set; }

    public string? EmisorVehicular { get; set; }

    public string? NroAutorizacion { get; set; }

    public string? Tipo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEdicion { get; set; }

    public int? EmpleadoIdcreacion { get; set; }

    public string? EmpleadoNombreCreacion { get; set; }

    public int? EmpleadoIdedicion { get; set; }

    public string? EmpleadoNombreEdicion { get; set; }

    public string? Estado { get; set; }

    public sbyte? IdEstado { get; set; }

    public virtual ICollection<UnidadTransporteGuid> TransporteGuids { get; set; } = new List<UnidadTransporteGuid>();
}
