using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaRestaurante_Admin.Models
{
    public class Combos
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int Estado { get; set; }

        [Column("categoria_id")]
        public int CategoriaId { get; set; }  
        public Categoria Categoria { get; set; } 

    }
}
