using System.ComponentModel.DataAnnotations;

namespace SistemaRestaurante_Admin.Models.Cargos
{
    public class cargo
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage = "Ingrese el cargo.")]
        public string nombre { get; set; }
        public int estado { get; set; }
    }
}
