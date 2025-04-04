using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaRestaurante_Admin.Models
{
    [Table("platos_combos")]
    public class Platos_Combos
    {
        [Key]
        public int Id { get; set; }
        [Column("combo_id")]
        public int ComboId { get; set; }
        public Combos Combo { get; set; }
        [Column("plato_id")]
        public int PlatoId { get; set; }
        public Platos Plato { get; set; }

        public int Estado { get; set; }
    }
}
