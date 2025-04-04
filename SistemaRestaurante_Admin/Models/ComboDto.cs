namespace SistemaRestaurante_Admin.Models
{
    public class ComboDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public List<int> PlatosSeleccionados { get; set; }
    }
}
