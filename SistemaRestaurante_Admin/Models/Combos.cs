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

        // Relación con Categoría
        [Column("categoria_id")]
        public int CategoriaId { get; set; }  // ID de la categoría
        public Categoria Categoria { get; set; }  // Propiedad de navegación

    }
}
