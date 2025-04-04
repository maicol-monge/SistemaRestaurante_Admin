namespace SistemaRestaurante_Admin.Models
{
    public class ComboDto
    {
        //Por favor no tocar esto, sirve para transportar los datos del backend del combo al frontend pli <3

        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public List<int> PlatosSeleccionados { get; set; }
    }
}
