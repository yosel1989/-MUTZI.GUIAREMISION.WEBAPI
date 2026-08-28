using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MUTZI.GUIA.DOMAIN.Entities.Catalogo
{
    [Table("vw_mdata_country")]
    public partial class Country
    {
        [Key]
        [Column("country_id")]
        public int CountryId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("code")]
        public string Code { get; set; }
    }
}
