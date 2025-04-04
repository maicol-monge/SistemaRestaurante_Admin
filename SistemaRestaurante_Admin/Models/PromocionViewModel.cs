using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaRestaurante_Admin.Models
{
    public class PromocionViewModel
    {
        [ValidateNever]
        public Promocion Promocion { get; set; } = new Promocion();

        [ValidateNever]
        public List<Combo> CombosDisponibles { get; set; }

        [ValidateNever]
        public List<Promocion> PromocionesActivas { get; set; }

        [ValidateNever]
        public List<Categoria> Categorias { get; set; }

        public bool EsEdicion => Promocion?.id > 0; 

    }
}
