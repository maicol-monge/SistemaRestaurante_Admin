using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaRestaurante_Admin.Models
{
    public class Promocion
    {
        public int id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "La descripción es requerido")]
        public string descripcion { get; set; }

        [Range(1, 100, ErrorMessage = "El descuento debe ser entre 1% y 100%")]
        public decimal? descuento { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        public int categoria_id { get; set; }

        public int estado { get; set; }

        [NotMapped] 
        public string CombosSeleccionados { get; set; }

        [NotMapped]
        public DateTime FechaInicioVista { get; set; }

        [NotMapped]
        public DateTime FechaFinVista { get; set; }

        [ValidateNever]
        public virtual Categoria Categoria { get; set; }

        [ValidateNever]
        public virtual ICollection<ComboPromocion> ComboPromociones { get; set; }
    }

    public class ComboPromocion
    {
        public int id { get; set; }
        public int combo_id { get; set; }
        public int promocion_id { get; set; }
        public DateTime fecha_inicio { get; set; }
        public DateTime fecha_fin { get; set; }
        public int estado { get; set; }

        public virtual Combos Combo { get; set; }
        public virtual Promocion Promocion { get; set; }
    }

    //public class Combo
    //{
    //    public int id { get; set; }
    //    public string nombre { get; set; }
    //    public string descripcion { get; set; }
    //    public decimal precio { get; set; }
    //    public int categoria_id { get; set; }
    //    public int estado { get; set; }
    //}
}
